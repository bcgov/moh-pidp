# A Sidecar for collecting/processing Logs

[Fluent-bit](https://docs.fluentbit.io/manual/about/what-is-fluent-bit) is basically a `log forwarder` tool that can be run as a sidecar container (a docker image) in each pod containing our apps, although it can also be deployed as a stand-alone service (servicing multiple apps). Fluent-bit can forward logs to lots of different outputs for example, HTTP, Opensearch, Slack, AWS Lamda and a lot more (see: https://docs.fluentbit.io/manual/pipeline/outputs). The steps required for deploying a Fluent-bit sidecar are shown below. Note: These steps are to deploy the sidecar.

You can read about a common-service-showcase in BCGOV which has deployed a Fluent-bit sidecar for CDOGS node application [here](https://github.com/bcgov/common-service-showcase/wiki/Logging-to-a-Sidecar).

## Implementing Fluent-bit for PIDP application on OpenShift

We deploy a Fluent-bit sidecar container in both backend/webapi and ingress/nginx applications to collect/process/monitor logs inside the apps and alert the PIDP team if certain keywords, regular expressions, etc. are matched in the log stream. For production ingress deployments, we use the non-debug image `docker.io/fluent/fluent-bit:3.2.10` (versioned in [`nginx-ver.properties`](../../nginx-ver.properties)); a `-debug` variant (`fluent-bit:3.2.10-debug`) is also available when busybox/bash tools are needed for troubleshooting. We forward alerts to a private Slack channel (a webhook). So our overall flow of logs is: 

`logs from nginx and webapi app > fluentbit sidecar > webhook (Slack channel)`.


### Fluent-bit for nginx/ingress app

Our nginx app outputs error logs to a configurable path (`/tmp/error.log`) configured in [server-block-configmap](../../charts/nginx/templates/server-block-configmap.yaml). Our Fluent-bit container mounts `/tmp/` from the nginx container and reads `/tmp/*.log`.
Fluent-bit has its own configuration file created via an OpenShift [configmap template](../../charts/nginx/templates/configmap.yaml). This config uses the `tail` plugin in the `INPUT` section to read `/tmp/*.log`, uses a `parsers` file (`Parsers_File parsers.conf`) to parse log formats, uses the `grep` plugin in the `FILTER` section to filter logs based on specific keywords (e.g. `error`, `crit`, `alert`, `emerg`), and uses the `SLACK` plugin in the `OUTPUT` section to send notifications to the PIDP Team's Slack channel.

### Fluent-bit for backend/webapi app

Our webapi app outputs logs to a configurable directory (`/app/logs/`) and the Fluent-bit container will mount this directory to a log-storage path. A [Fluent-bit configmap](/charts/webapi/templates/cofigmap.yaml) with the same configuration is used to read the log files `*.log` from the mounted path, filter the log streams for the keywords `ERR` and `FTL`, and output the logs to the PIDP Team's Slack channel. 
