# Steps to install CrunchyDB

1. oc login to cluster and choose the correct namespace:

<code>oc login --token=sha256~{redacted} --server=https://api.silver.devops.gov.bc.ca:6443</code>

<code>oc project abc123-dev</code>

2. context is important so be sure you're in the infra/crunchydb folder

<code>helm dependency update</code>

3. Install crunchy-postgres helm chart


<code>helm upgrade --install --values ./values.yaml --namespace d8a8f9-dev crunchy-postgres .</code>

# Steps to Migrate Databases and Roles from the old Crunchy/Patroni to the new Crunchy

1. Backup databases and roles from the old DB. In the old DB pod, run:

<code>pg_dump -C {DB_Name} > {outputDirectory/DB_Name.sql}</code>

<code>pg_dumpall --roles-only >  {outputDirectory/roles.sql}</code>

2. Zip the files using gzip command in the old pod

<code>gzip -9 {outputDirectory/DB_Name.sql}</code>

3. Copy zip files from the old pod to the local directory on your machine. In terminal on your local machine, run:


<code>oc rsync old-pod-name:remote-directory local-directory</code>

4. Copy zip files from your local directory to the destination pod.

<code>oc rsync local-directory new-pod-name:remote-directory</code>


5. Unzip zip files in the destination pod

<code>gzip -d {outputDirectory/DB_Name.sql.gz}</code>

6. Restore Roles and Databases respectively in the destination pod

<code>DROP DATABASE IF EXISTS {DB_NAME}</code>

<code>psql < {outputDirectory/roles.sql}</code>

<code>psql < {outputDirectory/DB_NAME.sql}</code>

7. Switch APIs to point the new crunchyDB