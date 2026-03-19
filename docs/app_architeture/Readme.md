# PIDP applications

PIDP applications are deployed as helm charts on the OpenShift platform. Webapi, frontend, plr-intake and nginx are PIDP applications and their helm charts are integrated with Git for direct deployment and they will be automatically updated using github workflows/pipelines when new changes are pushed to the develop , test and/or main branches.

The docker images are built and stored in OpenShift ImageStreams on the f088b1-tools namespace and image versions (e.g. dev, test, and prod) can be tracked. Sensitive data like API keys or credential is stored as a secret in OpenShift platform. For more information about how to deploy an application on OpenShift, refer to the official OpenShift documentation and the [B.C. government developer website](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/build-deploy-and-maintain-apps/deploy-an-application/).
