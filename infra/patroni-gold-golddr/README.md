## BCGov't Gold/GoldDR PSQL sample deployment
This strategy is a solution to provide multi-cluster high availability solution for Postgresql on the BC Govt's Openshift deployment on their Gold and GoldDR (as in Gold Disaster Recover so pronounce the D and R. We're leveraging the BC Gov't produced (patroni-postgres-container)[https://github.com/bcgov/patroni-postgres-container]. For more information, please refer to (cloudops/patroni-gold-golddr)[https://github.com/bashbang/cloudops/tree/main/patroni-gold-golddr] repository.

This example sets up a Patroni Cluster on Gold with a Helm chart. The same helm chart can be used to deploy on to GoldDR using the appropriate values files of course. The order of deployment is important. Gold must be done first, then GoldDR. This is because the TSC (see notes below on what a TSC is) is deployed with Gold only. The GoldDR service that's created when the TSC is needed in the helm deployment. So, in short, deploy Gold first, then GoldDR.

## Quick Start

```
# navigate to the helm folder:
cd infra/patroni-gold-golddr/helm/patroni-gold-golddr
# Login into Openshift Gold Cluster
oc login --token=sha256~{redacted} --server=https://api.gold.devops.gov.bc.ca:6443
# deploy Gold helm chart
helm upgrade --install --namespace f088b1-dev -f values-gold.yaml patroni .
# Login into Openshift GoldDR Cluster
oc login --token=sha256~{redacted} --server=https://api.golddr.devops.gov.bc.ca:6443
# deploy GoldDR helm chart
helm upgrade --install --namespace f088b1-dev -f values-golddr.yaml patroni .
```

## Things we learned and why we did stuff.
# Docker
We originally created a custom docker file for use that included OCP cli tool-set as well as a psql client, however we didn't want to have to manage all of that so we used stock off the shelf BC Gov't images for both and interwove the images. More details below.

# Patroni Configmaps
Patroni generally makes its own configmap for storing the Leader and Patroni config. This is defined in the DCS section. We opted to generate our own configmap of the same name through the Helm template. I did this for the sole purpose of giving control of those configmap assets to helm. It bothered me that when I did a helm delete that these assets were left behind and caused a conflict on future helm installs. This of course would not be an issue in a normal course of deployment, only really during development. However it's still tidy and can come in handy on a fallback after a failover occurs.

# Patroni init Pod
There's an init pod for the Patroni stateful set that obtains the TS port. This is done because at helm install time a custom BCGov't api called Transport Server Claim (TSC) is used to produce a Transport Service (TS). [Here's further details on how to setup](https://beta-docs.developer.gov.bc.ca/set-up-tcp-connectivity-on-private-cloud-openshift-platform/). The patroni config needs the port that this service runs on. We can only get it at runtime, not at Helm-time so this init container was created to get the TS port for use during the patroni container config at startup. The TS is a service running on Gold (and GoldDR) that acts as a tunnel by Patroni to send WAL data allowing GoldDRs cluster (which is a standby_cluster) to keep the PSQL data in sync. It remains as readonly until some action is taken to promote the "Standby Leader" to "Leader".

# Probe Monitor
An Openshift Cronjob was created on GoldDR to act as a Probe/Monitor to watch Gold's status. This proved to be a bit more complicated than we initially expected.
Our end result was a Cronjob that had two init containers. The first init container uses a stock OCP cli image to obtain the port that the TS runs on. This then gets stored as a file in an empty folder that then gets consumed by the second init container. The second init container is a PSQL image that probes Gold's PSQL through the TS to determine if it's running or not. The health status gets saved into the same empty folder then consumed by the third (main) container. This third container is again an OCP cli container that reads in the health status and if it's unhealthy updates GoldDR's patroni configmap to promote it to "Leader"

We explored various methods and ended up landing on having a "emptyDir: {}" which allowed us to communicate between the different containers in the pod.
# Alternate Probe Monitor
A github workflow was created that will monitor Gold for uptime. This workflow will determine if Gold is happy and if not, it'll update the config on GoldDR to promote it from "Standby Leader" to "Leader". We created and tested this, but didn't use it in favour of the Openshift Cronjob.

# Fail Back
There are no current plans to automate the switch back to Gold. However we did create a procedure and manual workflow to toggle the tag back to Gold from GoldDR. See the (FailBack.md)[./helm/patroni-gold-golddr/Failback.md] file

# Things discovered during development & deployment
- During the development of this chart the infrastructure was torn down and rebuilt from scratch each time. Things that didn't get torn down were the PVCs since they're generally holding DB data we'd not want to accidentally have them torn down in a production environment. This would have to be manually destroyed during development. From time to time we didn't tear down the PVC allowing PSQL to re-connect to the db. In these cases we would run into unusual behavour with the synchronization and sometimes would have challenges bring the DB up after it was deployed. We didn't look into this in any detail but are suspect that the WAL files have something to do with it. Possibly there's a state file being stored that's causing the issue. Once we move this initial implementation into a more real environment we expect we'll experience these problems again and be forced to do a deeper investigation.
- The replication password is an issue. Both Gold and GoldDR need to have the same "replication" password, however we don't have a good way of knowing that password between the two helm deployments. It's hard coded in the values files right now, but this is not optimal. The best idea we've had is to use the Vault Service (Platform Services offers a HashiCorp Vault service), however we don't have access to a Vault that could be used for testing/development.
- Setting the random password for the Postgres user wasn't straight forward. Have a look at the code. It's a bit complicated because we won't want the password to change each time we do a helm upgrade. [This was helpful](https://itnext.io/manage-auto-generated-secrets-in-your-helm-charts-5aee48ba6918)
- For BC Gov't implementations (which I expect would be the bulk of users reading this), it should be noted that as of this writing, a minimum size of a PVC is 20Mi and 1GB is allocated to each namespace. So be sure to take that into consideration when sizing your volumes as each patroni instance will have it's own storage of that defined size.
- I updated the labels with the hope of optimizing and organinzing them. That was a mistake after I got it working. It took me a long time to track down, but eventually I discovered that the label "app.kubernetes.io/name:" is super important to patroni. Patroni won't target updates properly without it.

# Assumptions
- This deals with the DB failover only. It does not consider an automated failback.
- This does not consider the application failover or failback. It assumes that the GSLB will be in place to fail over the application. There is a possibility the GSLB won't switch over the application, yet the PSQL will fail over to GoldDR. This could prove to be a problem. In theory if this were to happen Gold would continue to work so you'd not experience any issues, however it would sever the sync procedure and GoldDR would be stood up as a second Leader and of course then be out of sync with Gold. It would also do this silently. It would be a good idea to include an alert/alarm of some sort that would inform admins when a GoldDR failover occurred.
