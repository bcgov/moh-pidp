# Backup Deployments of Postgres
From [bcgov/backup-container](https://github.com/BCDevOps/backup-container), copy `openshift/templates`, `config` and `docker` folders. Together, the scripts and templates which will automatically deploy the backup app are provided in the openshift/templates directory. 
The following outlines the deployment of a simple backup of a PostgreSQL database to S3 Storage on OCP v4.x.

### Build backup-container image

First, in order to build an image and buildConfig in openshift, the [BuildConfig.yaml](./openshift/templates/BuildConfig.yaml) file is customized. 
1. The value of `git_repo_url`, `git_ref` and `source_context_dir` are set to `https://github.com/bcgov/moh-pidp.git`, `develop`, `/infra/backup-container/docker`, respectively. Start a new build once the buildConfig is deployed in openShift, it will create a new backup container image. 

2. Tag the backup container image. You can change the value of `global.image.tag` depending on the namespace you will deploy the backup-container app, which is `latest` by default.

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

1. Configure references to your DB credentials in [deployment.yaml](./openshift/templates/deployment.yaml), replacing the boilerplate `DATABASE_USER` and `DATABASE_PASSWORD` environment variables.
When using the [configmap.yaml](./openshift/templates/configmap.yaml) file the following environment variables are ignored, since you list all of your host/database pairs in the file; DATABASE_SERVICE_NAME, DATABASE_NAME. To provide the credentials needed for the listed databases you extend the deployment configuration to include hostname_USER and hostname_PASSWORD credential pairs which are wired to the appropriate secrets (where hostname matches the hostname/servicename, in all caps and underscores, of the database). For example, if you are backing up two database named prod-patroni/pidp_prod and metabase-database/metabase, you would have to extend the values configuration to include a PROD_PATRONI_USER and PROD_PATRONI_PASSWORD as well as METABASE_DATABASE_USER and METABASE_DATABASE_PASSWORD credential pairs, wired to the appropriate secrets, to access the database(s) on the prod-patroni server and metabase-dataset server.  Note that the key names within the database secret referencing database username and password are `app-db-username` and `app-db-password`, respectively. If this is not the case for your deployment, specify the correct key names as parameters `database_user_key_name` and `database_user_password_name` in [values.yaml](./openshift/values.yaml).

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

2. The backup container utilizes two volumes: one for storing the backups and another for restore/verification testing. The deployment template deliberately separates these volumes. The recommended storage class for the backup volume (backup_volume_class) for OCP4 is `netapp-file-backup`, backed up with the standard OCIO Backup infrastructure. Quota for this storage class is 2Gi by default. The default storage class for the restore/verification volume is `netapp-file-standard` (do not use netapp-file-backup as it is unsuitable for such transient workloads). The supplied deployment template will auto-provision this volume for you with it is published. Set the size of backup and verification PVCs by updating/overriding the value of `backup_volume_size` and `verification_volume_size` in [values.yaml](./openshift/values.yaml).

3. In this example, the namespace is `f088b1-prod` and the app name is `backup-container`. You can customize [values.yaml](./openshift/values.yaml) and change the value of parameters before creting backup container deployment.



<summary>Deploy backup-container helm chart</summary>

```bash
helm upgrade --install --values ./values.yaml --namespace f088b1-prod backup-container .
```

### Clean up the deployment and the image stream

```bash
helm uninstall --namespace f088b1-prod backup-container
```
