## Fail Back Procedure

TODO: This procedure needs to be properly tested and validated. This procedure is currently theoretical.

# To failback from GoldDR to Gold after an incident.
- Turn off Gold's PSQL to zero pods
- Drop the Gold PVC (or at least delete the data on the PVC)
- Update the patroni-config configmap in gold to add "standby_cluster":{"host": "patroni-master-golddr",
"port": $tsc_port, "username":"postgres", "password":""}} and remove the annotations and history. Remove the annotations in patroni-leader configmap as well. You can find a sample of this in (here)[./README.md]. It will likely need to be altered to use the correct TS as the names I think are different.
- turn on Gold's PSQL to 1 pod
- terminal into the PSQL and validate the config with ```patronictl list```. It should be `Standby Leader`.
- confirm the synchronization is occuring and/or complete
- once the sync is complete you'll need to work out the timing of the DB fail back and the application failback.
- to toggle the DB back to Gold, NULL the "standby_cluster" field in patroni-config configmap on GOLD (you can find a sample (here)[./README.md]). This will promote Gold back to being a Leader.
- At the same time, you'll need to get GoldDR back into sync. Add "standby_cluster":{"host": "patroni-master-gold",
"port": $tsc_port} to patroni-config on GOLDDR
- once patroni Gold is the leader and patroni GoldDR is the standby leader, you can update the midas-probe and add notouch=200 to the environment variables to force the return code to be 200 and manually control the GSLB to toggle from GoldDR to Gold. Once GSLB hits the midas-probe on Gold, you can remove notouch variable.