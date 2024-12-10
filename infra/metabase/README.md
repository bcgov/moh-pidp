# UPGRADE the version of Metabase if you’re running 45 or below

### 1. Download the latest version of Metabase image from docker hub

`docker pull metabase/metabase:latest`

### 2. Login to OpenShift
`oc login <token>`

### 3. Switch to the desired project
`oc project f088b1-tools`

### 4. Tag the downloaded image

`docker tag metabase/metabase:latest image-registry.apps.silver.devops.gov.bc.ca/f088b1-tools/metabase:<image-version>`

### 5. Login to docker and Push the image to the metabase imagestream in tools namespace

`docker login -u 'oc whoami' -p <password> image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools`

`docker push image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/metabase:<image-version>`

### 6. Point the latest image in metabase imagestream to the newly downlowded image

`oc tag metabase:<image-version> metabase:latest`


