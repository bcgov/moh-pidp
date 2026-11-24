## Fail Back From GOLDDR to GOLD (PostgreSQL 16 / Patroni 4.x)

For the full, up-to-date copy-paste failback runbook (including Patroni 4.x `Primary` vs legacy `master` terminology, suspending `patroni-probe` on GoldDR, and dynamic TSC port lookups), see [docs/distaster_recovery/Readme.md](../../../../docs/distaster_recovery/Readme.md).

### High-Level Summary
- Suspend the `patroni-probe` CronJob on GoldDR (`oc patch cronjob patroni-probe -p '{"spec":{"suspend":true}}'`).
- Take a safety backup from the live GoldDR `Primary` pod.
- Scale down Gold's Patroni StatefulSet to 0 pods, delete Gold's stale PVCs, clear `patroni-leader` annotations, and update Gold's `patroni-config` ConfigMap with `"standby_cluster":{"host":"patroni-master-golddr","port":<gold_tsc_port>}` (using Gold's live `patroni-master-golddr` service port).
- Scale up Gold's Patroni StatefulSet and verify sync with `patronictl -c /home/postgres/patroni.yml list` (`Standby Leader` + `Replica` pods, zero lag).
- Set `notouch=200` on Gold's `midas-probe` deployment and remove `standby_cluster` from Gold's `patroni-config` to promote Gold to **`Primary`** *(formerly `Leader`/`master` in Patroni < 4.0)*. Remove `notouch=200` once GSLB is routing to Gold.
- Scale down GoldDR's Patroni to 0, delete GoldDR's PVCs, configure GoldDR's `patroni-config` with `"standby_cluster":{"host":"patroni-master-gold","port":<dr_tsc_port>}`, scale GoldDR back up, verify `Standby Leader` status, and unsuspend `patroni-probe` on GoldDR.