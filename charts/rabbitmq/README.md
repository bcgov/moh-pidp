# Helm Charts

## Install Helm on your computer

See [Installing Helm](https://helm.sh/docs/intro/install/)


## RabbitMQ

### Create load definition secret 

rabbitmqsecret.yaml file was created to add a user other than rabbitmq and guest to rabbitmq-management, and set the permission.

`oc create -f ./rabbitmqsecret.yaml`


### Install helm chart


To install abbitmq chart from remote repository , use the regular helm install/upgrade command and specify the chart using url (bitnami website or docker hub) and obsolete the downloaded version (e.g. 11.10.0). You will need to supply the correct values file for your environemtn (using local path).

`helm install -f ./rabbitmq-values.yaml rabbitmq https://charts.bitnami.com/bitnami/rabbitmq-11.10.0.tgz`

in this deployment, the Clustering and Service Account are enabled in order to make it HA and the replica count is set to 3.

### Create router for rabbitmq-management console

After installing RabbitMQ helm chart, manually create a new router in OCP for rabbitmq service with following config:

Path: /
Target port: 15672
Security: Enabled
TLS termination: Edge
Insecure traffic: Redirect
