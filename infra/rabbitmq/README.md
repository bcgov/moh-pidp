# Helm Charts

## Install Helm on your computer

See [Installing Helm](https://helm.sh/docs/intro/install/)

## RabbitMQ

### Create load definition secret

rabbitmqsecret.yaml file was created to add a user other than **rabbitmq** and **guest** to rabbitmq-management, and set the permission. Run the following command using OpenShift credential in cmd:

`oc create -f ./rabbitmqsecret.yaml`

### Install helm chart

To install rabbitmq chart from remote repository:

* use the regular helm install/upgrade command and specify the chart using url (bitnami website or docker hub) and obsolete the downloaded version (e.g. 11.10.0). You will need to supply the correct values file for your environment (using local path).

`helm install -f ./values.yaml rabbitmq  oci://registry-1.docker.io/bitnamicharts/rabbitmq`

* or install from local directory:

`helm install -f ./values.yaml rabbitmq ./rabbitmq-13.0.2.tgz`

in this deployment, the Clustering and Service Account are enabled in order to make it HA and the replica count is set to three. The value of auth.password in the values.yaml file need to be updated.

### Create route for rabbitmq-management console

After installing RabbitMQ helm chart, manually create a new route in OCP for rabbitmq service with following config:

Name: rabbitmq-management
Service: rabbitmq
Path: /
Target port: http-stats (15672)
Security: Enabled
TLS termination: Edge
Insecure traffic: Redirect
