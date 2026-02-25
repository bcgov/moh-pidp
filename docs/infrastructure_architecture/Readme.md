
# PIDP applications
PIDP applications have been deployed as either helm chart or helm template on the OpenShift platform. Webapi, frontend, plr-intake and nginx helm charts are integrated with Git for direct deployment and they will be automatically updated using github workflows/pipelines once a PR is pushed to the develop branch, and then test and main branches. Infrastructure applications, including backup-container, fluentbit, mailhog, metabase,  midas-probe, patroni are designed for one-time setp and deployment and require updates only once a year.

The docker images are built and stored in OpenShift ImageStreams on the f088b1-tools namespace and image versions (e.g. dev, test, and prod) can be tracked. Sensitive data like API keys or credential is stored as a secret in OpenShift platform. For more information about how to deploy an application on OpenShift, refer to the official OpenShift documentation and the [B.C. government developer website](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/build-deploy-and-maintain-apps/deploy-an-application/).


## Backup Container
[BCGov Backup Container](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/database-and-api-management/database-backup-best-practices/) is a trusted backup utility tool, designed by BCGOV DevOps team build for developers working on the BCGov's OpenShift platform. It takes a full and complete backup of data into a single database dump file using pg_dump command every hour for the pidp_* databases. The backup files are stored on an nfs-file-backup PVC (persistent volume claim) on OpenShift platform. You can modify the backup cycle and schedule in the [backup-container-conf](infra\backup-container\chart\templates\configmap.yaml)

## Mailhog
MailHog is an email testing tool for developers:

 - Configure your application to use MailHog for SMTP delivery
 - View messages in the web UI, or retrieve them with the JSON API
 - Optionally release messages to real SMTP servers for delivery

## Midas-probe 
It checks if there's a failover incident from GOLD to GOLDDR. You need create a new slack channel to receive the notifications from midas and update the value of slack_webhook variable in midas-probe configmap if you 
Firstly and most importantly, you need to check the current IP address of OneHealthID application. If it returns 142.34.64.4, it means there's a failover and a manual failback from GOLD to GOLDDR is required.
