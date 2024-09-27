# Backup Deployments of Postgres
From [bcgov/backup-container](https://github.com/BCDevOps/backup-container), copy `openshift/templates`, `config` and `docker` folders. Together, the scripts and templates which will automatically deploy the backup app are provided in the openshift/templates directory. 
The following outlines the deployment of a simple backup of a PostgreSQL database to S3 Storage on OCP v4.x.

### Build backup-container image

First, in order to build an image and buildConfig in openshift, the [BuildConfig.yaml](./openshift/templates/BuildConfig.yaml) file is customized. 
1. The value of `git_repo_url`, `git_ref` and `source_context_dir` are set to `https://github.com/bcgov/moh-pidp.git`, `develop`, `/infra/backup-container/docker`, respectively.

2. Run the following command in cmd:
```bash
git clone https://github.com/bcgov/moh-pidp.git && cd infra\backup-container
```

3. Create a backup container image and tag it. You can change the value of `global.image.tag` depending on the namespace you will deploy the backup-container app, which is `latest` by default.

### Configure your DBs and set the cron schedule

Configure [configmap.yaml](./openshift/templates/configmap.yaml), list your database(s), make sure to specify the DatabaseType for each listed database, and set your cron schedule. Right now, the full backup is taken hourly and verification is 30 minutes after backup job is completed.

```bash
postgres=patroni-master:5432/pidp_prod

0 * * * * default bash ./backup.sh -s
30 * * * * default bash ./backup.sh -s -v all
```

### Backup Strategies
The backup app supports the rolling backup strategy. It allows you to keep a number of recent daily backups, a number of weekly backups, and a number of monthly backups. There are retention settings in [docker/backup.settings file](./docker/backup.settings). The defaults provide you with a week's worth of `daily` backups, a month's worth of `weekly` backups, and a single backup for the previous month. Backups are identified using a core name derived from the host/database specification and a timestamp. All backups are compressed using gzip.

### Deploy the app

1. Configure references to your DB credentials in [values.yaml](./openshift/values.yaml), replacing the boilerplate `DATABASE_USER` and `DATABASE_PASSWORD` environment variables.
When using the configmap.yaml file the following environment variables are ignored, since you list all of your host/database pairs in the file; DATABASE_SERVICE_NAME, DATABASE_NAME. To provide the credentials needed for the listed databases you extend the deployment configuration to include hostname_USER and hostname_PASSWORD credential pairs which are wired to the appropriate secrets (where hostname matches the hostname/servicename, in all caps and underscores, of the database). For example, if you are backing up two database named prod-patroni/prime_prod and metabase-database/metabase, you would have to extend the backup-deploy configuration to include a PROD_PATRONI_USER and PROD_PATRONI_PASSWORD as well as METABASE_DATABASE_USER and METABASE_DATABASE_PASSWORD credential pairs, wired to the appropriate secrets, to access the database(s) on the prod-patroni server and metabase-dataset server.  Note that the key names within the database secret referencing database username and password are `app-db-username` and `app-db-password`, respectively. If this is not the case for your deployment, specify the correct key names as parameters `DATABASE_USER_KEY_NAME` and `DATABASE_PASSWORD_KEY_NAME`.

```yaml
- name: DATABASE_USER
  valueFrom:
    secretKeyRef:
      name: "${DATABASE_DEPLOYMENT_NAME}"
      key: "${DATABASE_USER_KEY_NAME}"
- name: DATABASE_PASSWORD
  valueFrom:
    secretKeyRef:
      name: "${DATABASE_DEPLOYMENT_NAME}"
      key: "${DATABASE_PASSWORD_KEY_NAME}"
```

* Note that underscores should be used in the environment variable names.

2. The backup container utilizes two volumes: one for storing the backups and another for restore/verification testing. The deployment template deliberately separates these volumes. Because we are utilizing S3 storag, we use the `netapp-file-standard` storage class for both backup storage (BACKUP_VOLUME_CLASS) and restore/verification storage (VERIFICATION_VOLUME_CLASS). Set the value of `BACKUP_VOLUME_CLASS` in [backup-deploy.yaml](./openshift/templates/backup/backup-deploy.yaml) to netapp-file-standard.

3. In this example, the namespace is `f088b1-prod` and the app name is `backup-container`. You can customize [backup-deploy.yaml](./openshift/templates/backup/backup-deploy.yaml) and change the value of parameters before creting backup container deployment.
If you would like to utilize S3 storage and store backup files in a s3 bucket, create a secret in openshift for s3. The key names within the s3 secret referencing s3 username, password, endpoint and bucket are `S3_USER`, `S3_PASSWORD`, `S3_ENDPOINT`, and `S3_BUCKET`, respectively.



<summary>Create backup-container deployment</summary>

```bash
oc -n f088b1-prod process -f ./openshift/templates/backup/backup-deploy.yaml \
-p ENVIRONMENT_FRIENDLY_NAME='Patroni Backup' \
-p DATABASE_DEPLOYMENT_NAME=prod-patroni-secret -p ENVIRONMENT_NAME=f088b1-prod | oc -n f088b1-prod create -f -

```

### Clean up the deployment and the image stream

```bash
oc -n f088b1-prod delete pvc/backup-container pvc/backup-verification secret/backup-container secret/ftp-secret dc/backup-container networkpolicy/backup-container configmap/backup-conf
```


```bash
oc -n f088b1-prod delete buildconfig/backup-container imagestream/backup-container
```