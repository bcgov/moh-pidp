# UPGRADE the version of Metabase if you’re running 45 or below

### Download the latest version of Metabase image from docker hub

`docker pull metabase/metabase:latest``

### Login to OpenShift

### Switch to the desired project using oc project command

### Tag the downloaded image

`docker tag metabase/metabase:latest image-registry.apps.silver.devops.gov.bc.ca/<namespace>/metabase:<image-version>`

### Login to docker and Push the image to the metabase imagestream in tools namespace

`docker login -u 'oc whoami' -p <password> image-registry.apps.silver.devops.gov.bc.ca/<namespace>`

`docker push image-registry.apps.silver.devops.gov.bc.ca/<namespace>/metabase:<image-version>`

### Point the latest image in metabase imagestream to the newly downlowded image

`oc tag metabase:v0.47.2 metabase:latest`


