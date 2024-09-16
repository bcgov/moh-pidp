## Fail Back Procedure

TODO: This procedure needs to be properly tested and validated. This procedure is currently theoretical.

# To failback from GoldDR to Gold after an incident.
- Turn off Gold's PSQL to zero pods
- Drop the Gold PVC (or at least delete the data on the PVC)
- Update the patroni-config configmap in gold to add "standby_cluster":{"host": "patroni-master-golddr",
"port": $tsc_port}} and remove the annotations and history. Remove the annotations in patroni-leader configmap as well.(you can find a sample of this in (here)[./README.md]. It will likely need to be altered to use the correct TS as the names I think are different.
- turn on Gold's PSQL to 1 pod
- terminal into the PSQL and validate the config with ```patronictl list```
- confirm the synchronization is occuring and/or complete
- once the sync is complete you'll need to work out the timing of the DB fail back and the application failback.
- to toggle the DB back to Gold, NULL the "Standby Leader" field in patroni-config configmap on GOLD (you can find a sample (here)[./README.md]. This will promote Gold back to being a Leader
- At this point you'll need to get GoldDR back into sync. Add "standby_cluster":{"host": "patroni-master-gold",
"port": $tsc_port} to patroni-config on GOLDDR
- I feel the easies way is to just do a ```helm delete``` of the patroni chart on GoldDR, delete the PVC, then helm install the patroni chart into GoldDR again from scratch. This is basically the same init procedure we've run hundreds of times during development and has worked without issues.

TODO: Another possible method would be to run the values-golddr.yaml against the Gold namespace? This won't initially work as there is hard coded references to "Gold" and "Golddr" referencing the TS.  However, future consideration could be given to making this a value variable allowing it to be altered or overridden during install time. If this method works, then to "failback" it would be just a matter of doing a helm delete on Gold, then a helm install on Gold with some sort of param to identify it as "standby", wait for it to sync, then fail GoldDR to trigger the actual failback. Maybe a better way overall would be to remove the concept of "Gold" and "GoldDR" and just replace it with the concept of "Leader" and "Standby Leader" allowing the deployer to choose a role for either environment.