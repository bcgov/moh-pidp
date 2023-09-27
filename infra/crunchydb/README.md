# Steps to install CrunchyDB

1. oc login to cluster and choose the correct namespace:
<code>oc login --token=sha256~{redacted} --server=https://api.silver.devops.gov.bc.ca:6443
<code>oc project abc123-dev</code>

2. context is important so be sure you're in the infra/crunchydb folder
<code>helm dependency update</code>

3. Install crunchy-postgres helm chart
<code>helm upgrade --install --values values.yaml --namespace d8a8f9-dev crunchy-postgres .</code>
