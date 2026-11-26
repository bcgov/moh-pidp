Applications are typically served from the Gold cluster. Deployments in the GoldDR cluster are identical to Gold and are meant to be quickly activated as the live production system if there is a problem with the Gold cluster (failover). In the event of a failover from GOLD to GOLDDR, manual intervention is required to initiate the failback process.

> **Patroni 4.x / PostgreSQL 16 Notes:**
> - Starting in Patroni 4.x, the leader role and pod label were renamed from **`master`** to **`primary`** (`Primary` in `patronictl list`, `role: primary` on pods and the `patroni-master` Service selector). Standby clusters still show **`Standby Leader`** and **`Replica`**.
> - Always pass `-c /home/postgres/patroni.yml` when running `patronictl` inside a Patroni pod.
> - The PostgreSQL superuser is consistently `postgres` across all environments (`dev`, `test`, `prod`).
> - TSC ports (e.g., `16307`) are dynamically assigned by Porter when `patroni-postgres-tsc` is created and may differ between Gold (`patroni-master-golddr`) and GoldDR (`patroni-master-gold`). Always look up the live port on each cluster rather than hardcoding it.

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
   oc annotate cm patroni-config -n "$NS" initialize- --overwrite 2>/dev/null || true

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
    oc annotate cm patroni-config -n "$NS" initialize- --overwrite 2>/dev/null || true

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

---

# Cross-Cluster Replication Network Architecture & Troubleshooting

## How Traffic Flows Between GOLD and GOLDDR (Example IPs & Ports)

> **Note:** ClusterIPs, NodePorts (`30000–32767`), and Porter TSC ports are dynamically allocated and differ per environment (and can change if a Service or `TransportServerClaim` is recreated). The values below are real examples from `f088b1-test` / `f088b1-prod` for illustration.

Cross-cluster PostgreSQL replication (`pg_basebackup` + WAL streaming) does **not** connect directly to remote pod IPs or NodePorts. Instead, BC Gov's **Porter** operator (`TransportServerClaim` → F5 CIS `TransportServer` with label `f5cr: nodeport`) publishes a VIP port on the target cluster's F5 BIG-IP and creates a matching ClusterIP Service + Endpoint on the remote cluster:

```text
Normal Operation (GOLD Primary -> GOLDDR Standby):

GOLDDR patroni-0 (pod IP e.g. 10.95.15.253)
  └─► GOLDDR Service: patroni-master-gold (ClusterIP e.g. 10.96.72.172:40829 [test] / :16307 [prod])
        └─► GOLD F5 BIG-IP VIP (e.g. 142.34.229.61:40829 [test] / :16307 [prod])
              └─► GOLD OpenShift Worker Nodes NodePort (e.g. :31724 [test] / :30978 [prod])
                    └─► GOLD OVN-Kubernetes Gateway SNAT (source IP becomes 100.64.0.x)
                          └─► GOLD Service: patroni-master (type: NodePort, selector role=primary -> :5432)
                                └─► GOLD patroni-0 (pod IP e.g. 10.97.10.160:5432)
```

*(During failback from GOLDDR to GOLD, the exact mirror path is used in reverse: GOLD `patroni-0` → `patroni-master-golddr:<gold_tsc_port>` → GOLDDR F5 BIG-IP VIP → GOLDDR worker `NodePort` → GOLDDR OVN SNAT `100.64.0.x` → GOLDDR `patroni-0:5432`.)*

### Network & IP Range Reference

| Subnet / Address | Role | Examples |
| :--- | :--- | :--- |
| **`10.97.0.0/16`** | GOLD OpenShift pod network | `10.97.10.160` (`patroni-0`), `10.97.47.21` (`patroni-1`) |
| **`10.98.0.0/16`** | GOLD OpenShift ClusterIP Service network | `10.98.70.233` (`test` `patroni-master`), `10.98.125.243` (`prod` `patroni-master`) |
| **`10.95.0.0/16`** | GOLDDR OpenShift pod network | `10.95.15.253` (`patroni-0`), `10.95.13.84` (`patroni-1`) |
| **`10.96.0.0/16`** | GOLDDR OpenShift ClusterIP Service network | `10.96.72.172:40829` (`patroni-master-gold`) |
| **`100.64.0.0/16`** | OpenShift **OVN-Kubernetes** internal node/join SNAT subnet | `100.64.0.8` (source IP seen by PostgreSQL for incoming `NodePort`/F5 BIG-IP traffic) |
| **`142.34.229.61`** | GOLD F5 BIG-IP TSC VIP (replication tunnel) | `:40829` (`test`), `:16307` (`prod`) |
| **`142.34.229.4` / `142.34.64.4`** | GSLB HTTPS ingress VIPs | GOLD (`142.34.229.4:443`), GOLDDR (`142.34.64.4:443`) |
| **`142.34.229.245` / `142.34.94.245`** | Authoritative BC Gov GSLB DNS servers (`glb.gov.bc.ca`) | Resolves `*.healthprovideridentityportal.gov.bc.ca.glb.gov.bc.ca` |

## Why Each `host replication` Entry Exists in `pg_hba.conf`

In [`infra/patroni-gold-golddr/helm/patroni-gold-golddr/templates/configmap.yaml`](../../infra/patroni-gold-golddr/helm/patroni-gold-golddr/templates/configmap.yaml), `pg_hba` explicitly permits replication from four scopes — **all four are required** for Patroni 4.x on OpenShift OVN-Kubernetes:

- **`local replication all trust` & `127.0.0.1/32` / `::1/128` (`trust`):** Patroni 4.x opens a local replication connection to `localhost:5432` as the `replication` user to query local timeline and LSN (`get_replica_timeline`). Without these, standby/replica pods log `FATAL: no pg_hba.conf entry for replication connection from host "127.0.0.1", user "replication"`.
- **`10.97.0.0/16` (`scram-sha-256` / `md5`):** Allows in-cluster streaming replication between GOLD Patroni pods (`patroni-0` ↔ `patroni-1` / `patroni-2`).
- **`10.95.0.0/16` (`scram-sha-256` / `md5`):** Allows in-cluster streaming replication between GOLDDR Patroni pods.
- **`100.64.0.0/16` (`scram-sha-256` / `md5`):** **Critical for cross-cluster replication on OVN-Kubernetes.** Because F5 CIS (`f5cr: nodeport`) delivers cross-cluster TSC traffic to `service/patroni-master` (`type: NodePort`, `externalTrafficPolicy: Cluster`), OpenShift OVN-Kubernetes SNATs the source IP through the node's internal OVN join subnet (`100.64.0.0/16`, e.g., `100.64.0.8`) before it reaches the Patroni pod. Without `100.64.0.0/16`, `pg_basebackup` and WAL streaming across GOLD ↔ GOLDDR fail with `FATAL: no pg_hba.conf entry for replication connection from host "100.64.0.x", user "replication"`.

## Troubleshooting Porter `TransportServerClaim` / F5 BIG-IP `422`

- **`service/patroni-master` must stay `type: NodePort`** (configured in [`services.yaml`](../../infra/patroni-gold-golddr/helm/patroni-gold-golddr/templates/services.yaml)). Reverting it to `ClusterIP` causes Kubernetes to release the NodePort and F5 CIS (`f5cr: nodeport`) to fail with HTTP `422`.
- **Force Porter / F5 CIS to re-reconcile without deleting the TSC:** If `oc get transportserver patroni-postgres-tsc -n "$NS" -o jsonpath='{.status}{"\n"}'` reports `status: ERROR` (`Big-IP Responded with error code: 422`) or BIG-IP fails to pick up a `patroni-master` NodePort change, **do not delete the `TransportServerClaim`** (which tears down the VIP/port). Instead, toggle a harmless field such as `spec.monitor.timeout` (`10` → `11` → `10`) on the cluster hosting the `TransportServerClaim` to trigger operator reconciliation:
  ```bash
  oc patch transportserverclaim patroni-postgres-tsc -n "$NS" --type merge -p '{"spec":{"monitor":{"timeout":11}}}'
  sleep 5
  oc patch transportserverclaim patroni-postgres-tsc -n "$NS" --type merge -p '{"spec":{"monitor":{"timeout":10}}}'
  sleep 10
  oc get transportserver patroni-postgres-tsc -n "$NS" -o jsonpath='{.status}{"\n"}'
  ```
  Verify the status flips back to `{"status":"OK","vsAddress":"142.34.229.61",...}`.
