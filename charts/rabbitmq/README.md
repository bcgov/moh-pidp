# Helm Charts

## Install Helm on your computer

See [Installing Helm](https://helm.sh/docs/intro/install/)


## RabbitMQ

### Create load definition secret 

rabbitmqsecret.yaml file was created to add a user other than rabbitmq and quest to rabbitmq-management, and set the permission.

`oc create -f ./rabbitmqsecret.yaml`


### Install helm chart

To install from `tgz`, use the regular helm install/upgrade command and specify the chart using local path.
You will need to supply the correct values file for your environemtn.


`helm install rabbitmq .\rabbitmq-11.10.0.tgz --values .\rabbitmq-values.yaml`

To install from remote repository , use helm install/upgrade command and specify the chart using url (bitnami website or docker hub) considering the version. You will need to supply the correct values file for your environemtn.

`helm install -f ./rabbitmq-values.yaml rabbitmq https://charts.bitnami.com/bitnami/rabbitmq-11.10.0.tgz`



