# Metabase Deployment Guide

## 1. Push Metabase Docker Image to OpenShift ImageStream

### 1. Download the latest version of Metabase image from Docker Hub
```bash
docker pull metabase/metabase:latest
```

### 2. Login to OpenShift
```bash
oc login <cluster-url> --token=<token>
```

### 3. Switch to the tools project
```bash
oc project f088b1-tools
```

### 4. Tag the downloaded image
```bash
docker tag metabase/metabase:latest image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/metabase:<image-version>
```

### 5. Login to docker and push the image to the metabase imagestream in tools namespace
```bash
docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.apps.gold.devops.gov.bc.ca
docker push image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/metabase:<image-version>
```

### 6. Point the latest tag in metabase imagestream to the newly downloaded image
```bash
oc tag metabase:<image-version> metabase:latest -n f088b1-tools
```

---

## 2. Copy / Import PostgreSQL 16 Image into `f088b1-tools`

Choose **Option A** (direct OpenShift import — recommended) or **Option B** (Docker pull & push):

### Option A: Tag from OpenShift's Built-In `openshift` Namespace (Recommended)
Check if `16-el9` is already available in the cluster's `openshift` namespace (backed by Red Hat's `registry.redhat.io/rhel9/postgresql-16` image):
```bash
oc get is postgresql -n openshift
oc tag openshift/postgresql:16-el9 f088b1-tools/postgresql:16
oc tag openshift/postgresql:16-el9 f088b1-tools/postgresql:16-el9
```

### Option B: Direct OpenShift ImageStream Import from Quay.io (Public SCLorg Image)
If `16-el9` is not in the `openshift` namespace, import Red Hat SCLorg's public PostgreSQL 16 image directly into `f088b1-tools`:
```bash
# Switch to tools project
oc project f088b1-tools

# Import PostgreSQL 16 from Quay.io (public Red Hat SCLorg CentOS Stream 9 image)
oc import-image postgresql:16 --from=quay.io/sclorg/postgresql-16-c9s:latest --confirm -n f088b1-tools

# (Optional) Add 16-el9 alias
oc tag postgresql:16 postgresql:16-el9 -n f088b1-tools
```

### Option C: Docker Pull & Push
If importing directly is restricted by firewall or cluster egress:
```bash
# 1. Pull the official SCLorg image
docker pull quay.io/sclorg/postgresql-16-c9s:latest

# 2. Tag for the OpenShift internal registry via external route
docker tag quay.io/sclorg/postgresql-16-c9s:latest image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/postgresql:16

# 3. Login to registry
docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.apps.gold.devops.gov.bc.ca

# 4. Push image
docker push image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/postgresql:16

# 5. (Optional) Alias tag
oc tag postgresql:16 postgresql:16-el9 -n f088b1-tools
```

---

## 3. Deploying Metabase Helm Chart

### 1. Switch to the desired environment namespace (dev, test or prod)
```bash
oc project f088b1-dev   # or test / prod
```

### 2. Run helm upgrade command in `infra/metabase` directory
```bash
helm upgrade --install --values ./chart/values.yaml metabase chart
```

