Applications are typically served from the Gold cluster. Deployments in the GoldDR cluster are identical to Gold and are meant to be quickly activated as the live production system if there is a problem with the Gold cluster (failover). In the event of a failover from GOLD to GOLDDR, manual intervention is required to initiate the failback process.

> **Patroni 4.x / PostgreSQL 16 Notes:**
> - Starting in Patroni 4.x, the leader role and pod label were renamed from **`master`** to **`primary`** (`Primary` in `patronictl list`, `role: primary` on pods and the `patroni-master` Service selector). Standby clusters still show **`Standby Leader`** and **`Replica`**.
> - Always pass `-c /home/postgres/patroni.yml` when running `patronictl` inside a Patroni pod.
> - The PostgreSQL superuser is consistently `postgres` across all environments (`dev`, `test`, `prod`).
> - TSC ports (e.g., `16307`) are dynamically assigned by Porter when `patroni-postgres-tsc` is created and may differ between Gold (`patroni-master-golddr`) and GoldDR (`patroni-master-gold`). Always look up the live port on each cluster rather than hardcoding it.

# Automated Failback (GitHub Actions)

The preferred way to fail back from **GOLDDR** to **GOLD** is the manual GitHub Actions workflow **Patroni Failback - GOLDDR to GOLD** ([`.github/workflows/failback-golddr-to-gold.yaml`](../../.github/workflows/failback-golddr-to-gold.yaml)), backed by [`infra/patroni-gold-golddr/scripts/failback-golddr-to-gold.sh`](../../infra/patroni-gold-golddr/scripts/failback-golddr-to-gold.sh):

1. Open **Actions → Patroni Failback - GOLDDR to GOLD → Run workflow**.
2. First run with **`dry_run: true`** on `test` or `prod` — this verifies authentication, RBAC permissions (`oc auth can-i`), live Patroni roles, TSC ports, `midas-probe`/GSLB DNS, and the in-cluster `backup-container` PVC target with zero changes.
3. Run again with **`dry_run: false`** and **`confirmation`** set to `failback-test` or `failback-prod`.
   - The workflow orchestrates Phases 1–4 end-to-end across both clusters using isolated kubeconfigs, streams the safety `pg_dump` directly to the in-cluster `backup-container` `/backups/failback/` PVC (never uploaded to GitHub Actions), waits for `0 MB` replication lag before auto-promoting GOLD, toggles `midas-probe` `notouch=200` until GSLB DNS flips to GOLD (`142.34.229.4`), and re-syncs GOLDDR as `Standby Leader` before unsuspending `patroni-probe`.

*(If GitHub Actions is unavailable during an incident, you can either run `infra/patroni-gold-golddr/scripts/failback-golddr-to-gold.sh` locally with `KUBE_GOLD` and `KUBE_GOLDDR` set, or follow the manual runbook below.)*

# Steps to do manual failback from GOLDDR to GOLD

Set target environment variables first (adjust `ENV` for `dev`, `test`, or `prod`):
```bash
export ENV="prod"
export NS="f088b1-${ENV}"
export DB_NAME="pidp_${ENV}"
export TS="$(date +%Y%m%d_%H%M%S)"
```

### Phase 1: Prepare & Backup on GOLDDR (Current Live Primary)
1. **Suspend the automatic failover CronJob on GOLDDR** so it does not auto-promote GoldDR back to `Primary` during failback:
   ```bash
   oc patch cronjob patroni-probe -n "$NS" -p '{"spec":{"suspend":true}}'
   ```
2. **Take a safety backup from the live GOLDDR Primary pod** (streamed directly to your local machine):
   ```bash
   DR_PRIMARY=$(oc get cm patroni-leader -n "$NS" -o jsonpath='{.metadata.annotations.leader}')
   oc exec -n "$NS" "$DR_PRIMARY" -- \
     pg_dump -U postgres -Fc --no-owner --no-privileges "$DB_NAME" \
     > "${DB_NAME}_failback_${TS}.dump"
   ls -lh "${DB_NAME}_failback_${TS}.dump"
   ```

### Phase 2: Re-initialize GOLD as Standby & Sync from GOLDDR
3. **Scale down the Patroni StatefulSet on GOLD to 0** and delete its stale PVCs so it can perform a clean basebackup from GOLDDR:
   ```bash
   oc scale statefulset patroni -n "$NS" --replicas=0
   oc delete pvc -n "$NS" -l app.kubernetes.io/name=patroni
   ```
4. **Clear stale leader annotations and configure GOLD's `patroni-config` as a `standby_cluster`** pointing to GOLDDR via GOLD's live TSC port:
   ```bash
   GOLD_TSC_PORT=$(oc get svc patroni-master-golddr -n "$NS" -o jsonpath='{.spec.ports[0].port}')
   echo "GOLD -> GOLDDR TSC port: $GOLD_TSC_PORT"

   oc annotate cm patroni-leader -n "$NS" initialize- leader- optime- --overwrite 2>/dev/null || true

   oc patch cm patroni-config -n "$NS" --type merge -p "{
     \"metadata\": {
       \"annotations\": {
         \"config\": \"{\\\"postgresql\\\":{\\\"use_pg_rewind\\\":true,\\\"parameters\\\":{\\\"max_connections\\\":100,\\\"max_prepared_transactions\\\":0,\\\"max_locks_per_transaction\\\":64}},\\\"standby_cluster\\\":{\\\"host\\\":\\\"patroni-master-golddr\\\",\\\"port\\\":${GOLD_TSC_PORT}}}\"
       }
     }
   }"
   ```
5. **Scale up the Patroni StatefulSet on GOLD** (3 replicas in `prod`: 1 `Standby Leader` and 2 `Replicas`; 1 replica in `dev`):
   ```bash
   oc scale statefulset patroni -n "$NS" --replicas=3
   ```
6. **Verify replication sync on GOLD** once pods are Ready:
   ```bash
   oc exec -n "$NS" patroni-0 -- patronictl -c /home/postgres/patroni.yml list
   ```
   Confirm there is one `Standby Leader` and two `Replica` pods with `Lag in MB` at `0`.

### Phase 3: Promote GOLD to Primary & Switch Traffic
7. **Coordinate traffic cutover and promote GOLD:**
   - Add `notouch=200` to the `midas-probe` deployment environment variables on **GOLD** to redirect GSLB traffic to GOLD.
   - At the same time, remove `standby_cluster` from GOLD's `patroni-config` ConfigMap to promote GOLD from `Standby Leader` to **`Primary`** *(formerly `master` in Patroni < 4.0)*:
     ```bash
     oc patch cm patroni-config -n "$NS" --type merge -p '{
       "metadata": {
         "annotations": {
           "config": "{\"postgresql\":{\"use_pg_rewind\":true,\"parameters\":{\"max_connections\":100,\"max_prepared_transactions\":0,\"max_locks_per_transaction\":64}}}"
         }
       }
     }'
     ```
   - Verify GOLD is now **`Primary`** and `patroni-master` Service has an endpoint:
     ```bash
     oc exec -n "$NS" patroni-0 -- patronictl -c /home/postgres/patroni.yml list
     oc get endpoints patroni-master -n "$NS"
     ```
8. **Remove `notouch=200`** from the `midas-probe` deployment on GOLD once GSLB is routing traffic to GOLD.

### Phase 4: Re-sync GOLDDR as Standby Leader
9. **Scale down Patroni on GOLDDR to 0** and delete its old PVCs:
   ```bash
   oc scale statefulset patroni -n "$NS" --replicas=0
   oc delete pvc -n "$NS" -l app.kubernetes.io/name=patroni
   ```
10. **Clear stale leader annotations and configure GOLDDR's `patroni-config` as a `standby_cluster`** pointing back to GOLD via GOLDDR's live TSC port:
    ```bash
    DR_TSC_PORT=$(oc get svc patroni-master-gold -n "$NS" -o jsonpath='{.spec.ports[0].port}')
    echo "GOLDDR -> GOLD TSC port: $DR_TSC_PORT"

    oc annotate cm patroni-leader -n "$NS" initialize- leader- optime- --overwrite 2>/dev/null || true

    oc patch cm patroni-config -n "$NS" --type merge -p "{
      \"metadata\": {
        \"annotations\": {
          \"config\": \"{\\\"postgresql\\\":{\\\"use_pg_rewind\\\":true,\\\"parameters\\\":{\\\"max_connections\\\":100,\\\"max_prepared_transactions\\\":0,\\\"max_locks_per_transaction\\\":64}},\\\"standby_cluster\\\":{\\\"host\\\":\\\"patroni-master-gold\\\",\\\"port\\\":${DR_TSC_PORT}}}\"
        }
      }
    }"
    ```
11. **Scale up Patroni on GOLDDR** and verify it syncs as `Standby Leader`:
    ```bash
    oc scale statefulset patroni -n "$NS" --replicas=3
    oc exec -n "$NS" patroni-0 -- patronictl -c /home/postgres/patroni.yml list
    ```
    *(Do **not** promote GOLDDR or set `notouch=200` on GOLDDR.)*
12. **Unsuspend the `patroni-probe` CronJob on GOLDDR** once GOLD is `Primary` and GOLDDR is healthy as `Standby Leader`:
    ```bash
    oc patch cronjob patroni-probe -n "$NS" -p '{"spec":{"suspend":false}}'
    ```
