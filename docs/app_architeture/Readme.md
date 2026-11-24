# PIDP applications

PIDP applications are deployed as Helm charts on the OpenShift platform. Webapi (.NET 10), frontend (Angular 22 on `docker.io/nginx/nginx-unprivileged`), plr-intake (.NET 10), and ingress nginx (`docker.io/nginx/nginx-unprivileged`) are PIDP applications whose Helm charts are integrated with Git for automated deployment via GitHub Actions workflows when changes are pushed to `develop`, `test`, and `main`.

Container image versions for NGINX (and its metrics/Fluent Bit sidecars) are centrally managed in [`nginx-ver.properties`](../../nginx-ver.properties), while PostgreSQL 16 and Patroni 4.1.5 versions are tracked in [`postgres-ver.properties`](../../postgres-ver.properties).

The docker images are built and stored in OpenShift ImageStreams on the `f088b1-tools` namespace and image versions (e.g. `develop`, `test`, and `main`) can be tracked. Sensitive data like API keys or credentials are stored as secrets in the OpenShift platform. For more information about how to deploy an application on OpenShift, refer to the official OpenShift documentation and the [B.C. government developer website](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/build-deploy-and-maintain-apps/deploy-an-application/).
