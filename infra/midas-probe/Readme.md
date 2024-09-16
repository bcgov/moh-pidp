# Midas Probe - Openshift GSLB Keepalive Service for Gold/GoldDR Failover

This service is used to monitor several services running on Openshift (OCP). It has two primary purposes:
1. It will display the results of the services (running or not) based on the availability of pods for that service. In essence it depends on the OCP deployment readyness probes.
2. It acts as a keepalive for the BC Gov'ts Global Server Load Balancer (GSLB). It runs on the Gold cluster and if any of the services monitored by Midas does not have available pods, Midas will return a 500 code to the GSLB (when configured) resulting with the GSLB switching the DNS from pointing to Gold to GoldDR (pronounced Golder) for a failover.

## How does it work
The main function of this service is main.py. This python script uses a kubernetes library to look for any services with the tag: "midas=touch". For each of those services it determines if there's active pods for that service. If there are not, it will fail with a 500 code and display the list of service showing which one(s) failed. Midas on Gold, which is self aware of 'cluster' and is also aware of who the GSLB is currently pointing to: "who_is_active", force a non-200 return code when it sees that these two values are not the same.

Note: There's a environment variable that you can set "notouch" that will allow you to override the results. This is for testing purposes to allow you to manually control the toggle.  Accepted values for notouch are 200 or 500 depending on the return code you wish to force.

## Pipeline
There are two pipelines:
1. Build Midas Probe: this uses the docker-compose to build the docker image for release into the OCP imagestream on the -tools namespace.
2. Helm Upgrade Midas Probe: This does a helm upgrade (helm install needs to have previously been done manually). There's an integration into the Vault that Platform services runs. The private key, public key and certs are all stored in the vault. When your Personal Access Token (PAT) obtained from the user config within Vault is stored as a secret in Github Actions (GHA) this workflow will call to Vault, obtain the secrets and store them as files on the container, then helm will consume those files for deploying the route needed by the GSLB to communicate with Midas.  It should be noted that this PAT expires every 32 days so needs to be updated regularly.

## GSLB Setup Notes:
The config of the GSLB in the case of Midas-Probe is:

- URL or Common Name: www-dev.domain.com
- Back-end IP(s) and port:

| Cluster | IP:PORT          |
| :---:     | :---:              |
| GOLD    | 142.34.229.4:443 |
| GOLDDR  | 142.34.64.4:443  |

- Pool Configuration
  - enable Passive
  - Keepalive File URL: GET / HTTP/1.1\r\nHost:https://www-dev.domain.com/midas/touch\r\nConnection: Close\r\n\r\n
  - Keepalive File String: {empty}
  - Specify the load balancing method: choose "Active/Passive - Active Site: GOLD 142.34.229.4:443"


  In order to shutdown main app in midas, you can run the following command in midas-probe terminal: