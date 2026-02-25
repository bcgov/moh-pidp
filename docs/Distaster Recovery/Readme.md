## Steps to do failback from GOLDDR to GOLD
### On GOLD Cluster:
1.	Backup from the pidp database. Run the following commands in one of patroni pods on OpenShift:
`pg_dump -C pidp_prod>  /tmp/backups/pidp_prod.sql`
`gzip -9 /tmp/backups/pidp_prod.sql`
Copy the backup file to local directory. Run the following command in cmd on your local machine:
`oc rsync patroni-0:/tmp/backups <local_directory>`
2.	Scale down Patroni statefulset to zero pod and delete the corresponding Patroni PVC storage.
3.	Update patroni-config configmap and make it as a standby leader by replacing "null" with "standby_cluster".
4.	Only on the test environment, Change the superuser username to "postgres" in the test-patroni secret on both GOLD and GOLDDR clusters. Scale up Patroni on GOLD and as soon as the pod created, Change the superuser username to “pidpsuper” on both clusters to make the pod in the ready stat.
7.	On the production environment, scale up Patroni statefulset to three pods/replicas, one as master and two as readonly or replica. Once both pods are in ready state, terminal into one of the pods and validate the config with ```patronictl list```. There should be one `Standby Leader` and two `Replicas`.
8.	 Add "notouch=200" environment variable to the midas-probe deployment to manually redirect traffic to GOLD. 
9. At the same time, update patroni-config configmap to make the standby leader as leader by replacing "standby_cluster" with "null".
10. Remove "notouch=200" environment variable from the midas-probe deployment once GSLB hits the midas-probe on GOLD. 

### On GOLDDR Cluster:
Repeat steps 2 to 8.