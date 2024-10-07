TODO Notes:
- the pvc is created here by helm, but there's also a template in the stateful set that wishes to create the PVC. Maybe this should be changed and only created with helm? Since we know we have redundant storage behind the PCV we don't really need to have a Volume for each patroni instance? Could we just have a single volume and share it between pod instances? The leader is the only one that can write and the reads are really only there for fallback. If the app were to use the read replicas to offload the leader I'd think that would only offload POD performance and not Storage performance, so sharing the same PVC would be acceptable. 
- the replication password is hard coded in the values files. Gold and GoldDR need to have the same replication uid/pwd. 
- look into root password. There seems to be issues when having Gold's root password differ from GoldDR's password. There were sync issues. They may need to be the same. Which does make sense since the sync is likely bringing over Gold's password into GoldDR. Please verify. If this is the case then Gold and GoldDR need to have the same root password.
- useful tip was to use this command: ```helm template --output-dir ../../kustomize/gold/base --values ./values-gold.yaml patroni-gold .``` to render the helm templates and turn them into complete manifest files. This could possibly be used as a base for a kustomize implementation needed for ArgoCD.
- in at least one case during a fresh init, the config-map wasn't updated in time for the stateful set to load in the configmap get-ts-port script. This caused the pod to stall. A manual delete got the SS to create a new pod which worked fine, but investigate ordering some of the asset creation.
- this is designed for illustrating replication and is NOT a replacement for backups (since a accidental or malicious drop of a table would be synchronized). However it would be nice to include a strategy for doing some basic backups. There are better solutions out there that optimize space with incremental backups leveraging the WAL files and stepped backups, but for our purposes, we keep it simple and deployed a [backup-container](/infra/backup-container/) application that produces a full backup every hour with pg_dumps and keep the last 7 days worth of files. You can simply run the following command as a cronjob to takes hourly backup of database. You need to pass in the database  name, uid, and password from env secrets. In order to backup and restore the databases, you need to connect to the db as super user before copying data

 ``` psql -U postgres {app_db_name} < /tmp/backup/pidp_prod.sql ```

```
#!/bin/bash
# run this cron as a job 0 */1 * * *
set -e
set -o pipefail


/location/of/command/pg_dump -U postgres -p"${root_password}" "${app_db_name}" | gzip > /backup/$(date +%F)_application.sql.gz

# cleann up files > 7 days
find /backup -type f -mtime +7 -delete > /dev/null 2>&1

```


# CLI help
OC Command to get the leader that's listed in the configmap:
```
leader=`oc get configmap patroni-leader -o "jsonpath={.metadata.annotations.leader}"|cut -c1-`
```
OC command to get patroni state of selected $leader:
```
state=`oc exec $leader -- curl -s http://localhost:8008/health | jq -r '.state'`
```

validate the role of the leader
```
oc exec $leader -- curl -s http://localhost:8008/health | jq -r '.role'
```

set the standby to be leader by removing the standby setting in the cluster config....this will make it the leader
```
oc get configmap patroni-config -o json | sed -e "s/standby_cluster/null/g" | oc replace -f -
```

This is used in a service container to obtain the port for the TSC that we can then inject into a secret so that patroni config can use it
```
tsc_port=`oc get services patroni-master-gold -o jsonpath={.spec.ports[0].port}`
```

Config for Gold
```
echo '{
    "apiVersion": "v1",
    "kind": "ConfigMap",
    "metadata": {
        "annotations": {
            "config": "{\"postgresql\":{\"use_pg_rewind\":true,\"parameters\":{\"max_connections\":100,\"max_prepared_transactions\":0,\"max_locks_per_transaction\":64}}}"
        },
        "labels": {
            "app.kubernetes.io/name": "patroni",
            "cluster-name": "patroni"
        },
        "name": "patroni-config",
        "namespace": "f088b1-dev"
    }
}' | oc replace -f -
```

Config for GoldDR - Note you need to set the $tsc_port variable first
```
tsc_port=`oc get services patroni-master-gold -o jsonpath={.spec.ports[0].port}`
 echo '{
    "apiVersion": "v1",
    "kind": "ConfigMap",
    "metadata": {
        "annotations": {
            "config": "{\"postgresql\":{\"use_pg_rewind\":true,\"parameters\":{\"max_connections\":100,\"max_prepared_transactions\":0,\"max_locks_per_transaction\":64}},\"standby_cluster\":{\"host\":\"patroni-master-gold\",\"port\":'$tsc_port'}}"
        },
        "labels": {
            "app.kubernetes.io/name": "patroni",
            "cluster-name": "patroni"
        },
        "name": "patroni-config",
        "namespace": "f088b1-dev"
    }
}' | oc replace -f -
```

This is the raw "Standby Leader" config. This was used a few times during testing to force a cluster into standby mode. It was also used on Gold to allow to fail back.
```
{"postgresql":{"use_pg_rewind":true,"parameters":{"max_connections":100,"max_prepared_transactions":0,"max_locks_per_transaction":64}},"standby_cluster":{"host":"patroni-master-gold","port":'$tsc_port''}}
```
