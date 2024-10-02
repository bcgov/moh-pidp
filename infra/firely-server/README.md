# Firely Server Helm Chart

## Prerequisites Details

* Kubernetes 1.19+
* PV support on the underlying infrastructure.
* Firely Server license

## Chart Details

This chart implements a [Firely Server](https://fire.ly/products/firely-server/) deployment on a [Kubernetes](https://kubernetes.io) cluster using the [Helm](https://helm.sh/) package manager.

## Installing the Chart


To install the chart with the release name `my-release`:

``` console
$ helm repo add firely-server https://raw.githubusercontent.com/FirelyTeam/Helm.Charts/main/firely-server/
$ helm install my-release firely-server/firely-server

```

Note: firely/server image requires root privilege to start. However, you can't run the pods as a root user on Openshift.You need to customize the firely/server docker image, create a non-root user and change the ownership of /app folder to the non-root user:

```
docker build -t firely/server .
docker run -it --rm --name firely-server  -v {$pwd}\firelyserver-license.json:/app/firelyserver-license.json firely/server --user non-root -e APP_USER={} APP_GROUP={}

```

The command deploys Firely Server on the Kubernetes cluster in the default configuration. The configuration section lists the parameters that can be configured during installation.

## Uninstalling the Chart
To uninstall/delete the `my-release` deployment:

``` console 
$ helm delete my-release
```

The command removes all the Kubernetes components associated with the chart and deletes the release.

## Configuration

The following table lists the configurable parameters of the firely-server chart and their default values.

### Global parameters

 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `image.repository`                                       | Firely Server image name                                                  | `firely/server`                                   |
| `image.tag`                                              | Firely Server image tag                                                   | `4.7.0`                                             |
| `image.pullPolicy`                                       | Firely Server image pull policy                                           |`IfNotPresent`                                       |

### Common parameters
 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `imagePullSecrets`                                       | Reference to one or more secrets to be used when pulling images    | `[]` |
| `nameOverride`                                           | String to partially override firely-server.fullname template (will maintain the release name) | `""`
| `fullnameOverride`                                       | String to fully override firely-server.fullname template | `""` |

### Security parameters

 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `podSecurityContext`                                      | Security settings for the Pod, like `fsGroup`.                           | `{}`                                   |
| `securityContext`                                         | Security settings for the container, like `allowPrivilegeEscalation` and `capabilities`.                                                   | `{}`                                             |

### Firely Server parameters
 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `vonksettings`                                           | The override of the appsettings of Firely Server. This must be a Json format. Do not forget to indent this with 2 white spaces. See also more information below.  | `{ }` |
| `envFromSecret`                                          | The name of an existing secret in the same kubernetes namespace which contains key-value pairs which are converted into environment variables of the Firely Server container. This is the recommended approach for specifying all confidential settings, like connection strings, etc. For example, run the following to create a secret containing a connection string for MSSQL: `kubectl create secret generic -n ${FS_NAMESPACE} vonk-env-secret --from-literal=VONK_SqlDbOptions__ConnectionString='User Id=...'`| `nil` |
| `extraEnvs`                                              | An array of environment variables expressed in a [standard way for Kubernetes pod](https://kubernetes.io/docs/tasks/inject-data-application/define-environment-variable-container/) as key-value pair to add to the container | `[]` |
| `logsettings`                                            | The override of the logsettings of Firely Server. This must be a Json format.  Do not forget to indent this with 2 white spaces. See also more information below.| `{ }` |
| `license.secretName`                                     | You can  save the Firely Server license key yourself as a Kubernetes secret. Use then the name of that secret here. This setting overrides `license.value`| `nil` |
| `license.fileName`                                       | The name of the license file used internally. No reason to change that. | `firelyserver-license.json` |
| `license.mountPath`                                      | The folder where the license is mounted. | `/var/run/license` |
| `license.mountedFromExtraVolumes`                        | If `true`, the license file is mounted from an extra volume, ignoring `license.Value` and `license.secretName` in this case. See more information below. | `false` |
  | `license.value`                                          | The content of the license. See also more information below. | `nil` |
| `license.requestInfoFile`                                | Sets the location of the file with request information. This file will be used in future releases.| `nil` |
| `license.writeRequestInfoFileInterval`                   | Sets the time interval (in minutes) to write aggregate information about processed requests to the RequestInfoFile.| `nil` |
| `plugins[].url`                                          | An absolute url where a Firely Server plugin can be downloaded| `nil` |
| `plugins[].checksum`                                     | The checksum of the plugin | `nil` |
| `extraVolumes`                                           | Array of additional volumes to be mounted in the Firely Server container | `[]` |
| `extraMountPoints`                                       | Array of additional mount points for the Firely Server container | `[]` |

### Firely Server deployment parameters 
 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `replicaCount`                                           | Number of replicas in the replica set                                     | `1`                                                 |
| `readinessProbe.initialDelaySeconds`                     | Number of seconds after the container has started before readiness probes are initiated | `15` |
| `readinessProbe.timeoutSeconds`                          | Number of seconds after which the probe times out  | `5` |
| `readinessProbe.failureThreshold`                        | When a Pod starts and the probe fails, Kubernetes will try `failureThreshold` times before giving up. The Pod will be marked Unready. | `3`| 
| `readinessProbe.periodSeconds`                           | How often (in seconds) to perform the probe | `10` |
| `readinessProbe.successThreshold`                        | Minimum consecutive successes for the probe to be considered successful after having failed | `1` |
| `livenessProbe.initialDelaySeconds`                      | Number of seconds after the container has started before liveness probes are initiated | `10` |
| `livenessProbe.timeoutSeconds`                           | Number of seconds after which the probe times out | `5` |
| `livenessProbe.failureThreshold`                         | When a Pod starts and the probe fails, Kubernetes will try `failureThreshold` times before giving up. The Pod will restart | `3` |
| `livenessProbe.periodSeconds`                            | How often (in seconds) to perform the probe | `120` |
| `livenessProbe.successThreshold`                         | inimum consecutive successes for the probe to be considered successful after having failed | `1` |
| `resources.limits`                                       | The resources limits for the Firely Server container | `{}`|
| `resources.requests`                                     | The requested resources for the Firely Server container | `{}` |



### Traffic Exposure Parameters
 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `service.type`                                           | Kubernetes Service type                                                   | `LoadBalancer` |
| `service.port`                                           | The port which the Kubernetes service will expose the Firely Server pod   | `80` |
| `ingress.enabled`                                        | Enable ingress record generation for Firely Server                        | `false` |
| `ingress.annotations`                                    | Annotations for this host's ingress record                                | `{}` |
| `ingress.className`                                      | Ingress Class Name that will be be used to implement the Ingress          | `nginx` |
| `ingress.certIssuer`                                     | The name of the cert-manager                                              | `letsencrypt-production` |
| `ingress.path`                                           | Path within the url structure                                             | `/` |
| `ingress.pathType`                                       | Ingress path type                                                         | `Prefix` |
| `ingress.hosts[0]`                                       | Hostname to your Firely Server installation                               | `nil` |
| `ingress.tls[0].secretName`                              | The secretname used in the cert-manager                                   | `nil` |

### Persistence Parameters
 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `persistence.enabled`                                    | When set to `true` a mount would be available in the Firely Server pod on path `/var/run/vonk` | `false` |
| `persistence.existingPVClaim`                            | The name of an existing PVC to use for persistence                        | `nil` |

### Firely Server Validation service Parameters

This validation service is experimental and is not supported yet. 

 Name                                                      | Description                                                               | Default                                             |
| -------------------------------------------------------- | ------------------------------------------------------------------------- | --------------------------------------------------- |
| `validation.enabled`                            | Enable the Firely Server Validation service. When `false`, Firely Server uses its internal validator, otherwise it will use this validation service| `false` |
| `validation.replicaCount`                       | Number of replicas in the replica set | `1`   |
| `validation.image.registry`                     | Firely Server Validation image name | `firely.azurecr.io`   |
| `validation.image.repository`                   | Firely Server Validation image respository | `firely/firely-cloud-components`   |
| `validation.image.tag`                          | Firely Server Validation image tag | `latest`   |
| `validation.image.pullPolicy`                   | Firely Server Validation image pull policy | `IfNotPresent`   |
| `validation.port`                               | The port which the Kubernetes service will expose the Firely Server Validation pod | `8080`   |
| `validation.persistence.carfilePVClaim`         | The PersistenceVolumeClaim where the carfiles are located  | `nil`   |
| `validation.resources`                          | The resources limits for the Firely Server Validation container | `nil`   |
| `validation.nodeSelector`                       | Allows you to constrain which nodes your pod is eligible to be scheduled on, based on labels on the node  | `{ }`   |
| `validation.affinity`                           | A more elaborate way to constrain which nodes your pod is eligible to be scheduled on| `{ }`   |
| `validation.tolerations`                        | Allow (but do not require) the pods to schedule onto nodes with matching taints | `{ }`   |
| `validationsettings`                            | The override of the appsettings of Firely Server Validation service. This must be a Json format. Do not forget to indent this with 2 white spaces | `{ }` |

### Opentelemetry-collector parameters
Name                                                      | Description                                                               | Default                                             |
| `opentelemetry-collector.enabled`                       | If `true`, enable the deployment of [opentelemetry-collector helm chart](https://github.com/open-telemetry/opentelemetry-helm-charts/blob/main/charts/opentelemetry-collector/README.md) | `false` |

In addition, all parameters from the `opentelemetry-collector` (see the [documentation](https://github.com/open-telemetry/opentelemetry-helm-charts/blob/main/charts/opentelemetry-collector/values.yaml)) can be modified by prefixing those parameters with the `opentelemetry-collector.` prefix.

### Telegraf parameters
Name                                                      | Description                                                               | Default                                             |
| `telegraf.enabled`                       | If `true`, enable the deployment of [telegraf helm chart](https://github.com/influxdata/helm-charts/blob/master/charts/telegraf/README.md) | `false` |

In addition, all parameters from the `telegraf` (see the [documentation](https://github.com/influxdata/helm-charts/blob/master/charts/telegraf/values.yaml)) can be modified by prefixing those parameters with the `telegraf.` prefix.

### InfluxDb2
Name                                                      | Description                                                               | Default                                             |
| `influxdb2.enabled`                       | If `true`, enable the deployment of [influxdb2 helm chart](https://github.com/influxdata/helm-charts/blob/master/charts/influxdb2/README.md) | `false` |

In addition, all parameters from the `influxdb2` (see the [documentation](https://github.com/influxdata/helm-charts/blob/master/charts/influxdb2/values.yaml)) can be modified by prefixing those parameters with th `influxdb2.` prefix.

## Obtaining and using a license

Download the the license file from [Simplifier.net](https://simplifier.net/firely-server).

Once you have a valid license file (for example `license.json`), you have multiple options for deploying it.

### Using helm value `license.value`

Transform the downloaded license in the following format and save it as `firelyserver-license.yaml' 

```yaml
license:
  value: '{"LicenseOptions": {"Kind": "Evaluation","ValidUntil": "2018-11-13","Licensee": "marco@fire.ly"}, "Signature": "+hLwZrrTL4tcW+l0r5yDHYSASM6EWiaVcRBN1..etc"}'
```
> **Note**: the option value should be on one line. No line feeds.

Install the firely-server chart like this:
```console
$ helm install my-release -f .\firelyserver-license.yaml firely-server/firely-server 
```

### Using a secret 
Create a secret in the Firely Server namespace with the following command:
```SH
kubectl create secret generic vonk-license --namespace ${FS_NAMESPACE} --from-file=firelyserver-license.json=./license.json`
```
Then, modify the `values.yaml` to add the following settings:
```YAML
...
license:
  secretName: vonk-license
...
```

### Using a keyvault
See the dedicated keyvault section below.

## Override Firely Server settings
There are multiple ways to specify Firely Server settings.
### Use `vonksettings`
You can override the default configuration settings of Firely Server by override the appsettings elements, like described in https://docs.simplifier.net/projects/Firely-Server/en/latest/configuration/appsettings.html. 

In the following example the `InformationModel` has been overriden. The default is now `Fhir4.0` instead of `Fhir3.0`, which is the default. Furthermore the minimal loglevel is set to `Information`.

```yaml
vonksettings: 
  {
    "InformationModel":{
      "Default":"Fhir4.0"
    }
  }

logsettings:
  {
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information"
      }
    }
  }
```

> **Note**: be carefull to indent the json snippet correctly! The first curly bracket should have been indented with 2 white spaces. The next element (in our example `"InformationModel"`) should be prefixed with 4 white spaces, etc.

> **Note**: don't use double forward slashes (`//`) to comment out sections. JSON formally has no notion of comments.

Install the firely-server Chart like this:
```console
$ helm install my-release -f .\settings.yaml firely-server/firely-server 
```

### Use a secret
For confidential settings, you mmight want to use a secret managed differently that the other settings. For this, you need to create a secret
in the Firely Server namespace using a command similar to 
```SH
kubectl create secret generic -n ${FS_NAMESPACE} vonk-env-secret --from-literal=VONK_SqlDbOptions__ConnectionString='User Id=...'`
```
And add the following settings in you `values.yaml`:
```YAML
envFromSecret: "vonk-env-secret"
```
Then, the environment variable `VONK_SqlDbOptions__ConnectionString` will be set in the Firely Server container, thereby overriding the default value from the `appsettings.json`.

Note that the secret could also contain additional settings.

### Use an Azure Keyvault
See below.


## Using Azure keyvault
By using an Azure keyvault, you can retrieve artifacts directly from a Keyvault and either mount it inside the Firely Server container or access them through using environment variables.
In order to achieve that, some pre-requisites must be fulfilled. You can find more details in [Azure doc](https://learn.microsoft.com/en-us/azure/aks/csi-secrets-store-identity-access).
Below, we will describe how the Azure keyvault could be used to retrieve the Firely license as well as the  connection string for accessing the MS SQL database. This can be extended to access additional confidential settings used in Firely Server.

First, you need to enable the keyvault add-in in the AKS cluster:
```SH
az aks enable-addons --resource-group $RESOURCE_GROUP --name $CLUSTER_NAME --addons azure-keyvault-secrets-provider
```
Then, you need to allow the client identity associated to the keyvault add-in to access your keyvault.
This can be done using a script similar to the following:
```SH
$kvProviderClientId = az aks show --resource-group $RESOURCE_GROUP  --name $CLUSTER_NAME --query addonProfiles.azureKeyvaultSecretsProvider.identity.clientId -o tsv
Write-Host "Identity Client Id of the user identity associated to keyvault add-on: $kvProviderClientId"
Write-Host "Adding the roles to access the Keyvault to the user client identity associated to keyvault ($kvProviderClientId)..."

foreach ($role in @("Key Vault Secrets User", "Key Vault Certificate User", "Key Vault Crypto User")) {
   $roleExists = az role assignment list --role $role --assignee $kvProviderClientId --scope $keyvaultId --query "[].id | [0]"
   if ($roleExists) {
      Write-Host "Role '$role' already assigned to '$kvProviderClientId'"
   }
   else {
      Write-Host "Role '$role' is not yet assigned to '$kvProviderClientId'. Assigning it..."
      az role assignment create --role "$role" --assignee $kvProviderClientId --scope $keyvaultId
      if ($? -eq $false) {
         Write-Host "Error assigning role '$role'."
      }
      else {
         Write-Host "Role '$role' assigned to '$kvProviderClientId'"
      }
   }
}
```

Once the keyvault add-on is enabled and the associated client has access to your keyvault, you need to 
deploy a `SecretProviderClass` in the Firely Server namespace, using the following command:
```SH
kubectl apply -n $FS_NAMESPACE ./azure_kv_spc.yaml
```
with `./azure_kv_spc.yaml` being similar to the following:
```YAML
apiVersion: secrets-store.csi.x-k8s.io/v1
kind: SecretProviderClass
metadata:
  name: $name
spec:
  provider: azure
  parameters:
    usePodIdentity: "false"
    useVMManagedIdentity: "true"
    userAssignedIdentityID: "<KEYVAULT_PROVIDER_CLIENTID>"
    keyvaultName: "<KEYVAULT_NAME>"
    cloudName: "AzurePublicCloud"   
    objects:  |
      array:
        - |
          objectName: <FS_MSSQL_CONNECTION_STRING_KEYVAULT_OBJECTNAME>
          objectType: secret
          objectVersion: "" 
        - |
          objectName: <FS_LICENSE_KEYVAULT_OBJECTNAME>
          objectType: secret
          objectVersion: "" 
    tenantId: "$tenantId"
  secretObjects:
      - secretName: firely-keyvault-secrets
        type: Opaque
        data:
          - key: <FS_MSSQL_CONNECTION_STRING_SECRET_DATAKEY>
            objectName: <FS_MSSQL_CONNECTION_STRING_KEYVAULT_OBJECTNAME>
"@
```
with:
- `<KEYVAULT_NAME>` is the name of the Azure keyvault
- `<KEYVAULT_PROVIDER_CLIENTID>` is the id of the client associated to the Keyvault add-in
- `<FS_LICENSE_KEYVAULT_OBJECTNAME>` is the name of the secret where the license content is stored in the Azure Keyvault
- `<FS_MSSQL_CONNECTION_STRING_KEYVAULT_OBJECTNAME>` is the name of the secret where the MSSQL connection string is stored in the Azure Keyvault
- `<FS_MSSQL_CONNECTION_STRING_SECRET_DATAKEY>` is the name of the entry in the Kubernetes secret created by the `SecretProviderClass`


In order to access the license stored in the keyvault from the Firely Server container, you need to specify that the license file should be read from a custom mount point corresponding to a custom volume by adding the following entries in your `values.yaml` used in the helm deployment:
```YAML
...
license:
  filename: <FS_LICENSE_KEYVAULT_OBJECTNAME>
  mountPath: /mnt/keyvault_secrets_store
  mountedFromExtraVolumes: true
...
extraVolumes:
  - name: keyvault-secrets-store
    csi:
      driver: secrets-store.csi.k8s.io
      readOnly: true
      volumeAttributes:
        secretProviderClass: "firely-keyvault"
extraMountPoints:
  - name: keyvault-secrets-store
    mountPath: /mnt/keyvault_secrets_store
    readOnly: true
...
```

In order to load the MS SQL connection string from Firely Server, add the following entries in your `values.yaml`:
```YAML
...
extraEnvs:
...
  - name: VONK_SqlDbOptions__ConnectionString
    valueFrom:
      secretKeyRef:
        key: <FS_MSSQL_CONNECTION_STRING_SECRET_DATAKEY>
        name: firely-keyvault-secrets
...
```

## Deploying Opentelemetry-collector, Telegraf and InfluxDb2
Since [release 5.7](https://docs.fire.ly/projects/Firely-Server/en/latest/releasenotes/releasenotes_v5.html#features), Firely Server provides functionality to run analytics queries on usage metrics collected via OpenTelemetry. This feature can be used to build reports for the ONC Real World Testing Condition and Maintenance of Certification requirement. See [Real World Testing](https://docs.fire.ly/projects/Firely-Server/en/latest/features_and_tools/realworldtesting.html#feature-realworldtesting) for more information.

At this time, Firely Server relies explicitely on `Opentelemetry-collector` to filter and forward the Opentelemetry traces towards `telegraf` which transforms those traces and stores the processed data in an `influxdb2` server. In order to achieve this, specific configurations are required for each of those services, and are mentioned in the [Firely Server documentation](https://docs.fire.ly/projects/Firely-Server/en/latest/features_and_tools/realworldtesting.html#feature-realworldtesting).

We stringly Firely Server who wish to enable the Real World Testing to deploy each of those services independently in order to provide the required security and reliability matching their use case.

However, in order to simplify testing this feature, we have added a dependency towards the helm charts deploying those services and those can therefore be deployed
as part of the Firely Server deployment when enabling that.
In order to enable the deployment, the following settings must be set when deployment Firely Server helm chart:
```YAML
opentelemetry-collector:
  enabled: true
telegraf:
  enabled: true
influxdb2:
  enabled: true
```
The deployment of those dependencies can be customized by modifying the parameters as mentioned earlier, by simply prefixing the original parameter with the name of 
the dependency. For example, docker image used for the `influxdb2` deployment can be modified as follows:
```YAML
influxdb2:
  image:
    tag: 2.7.4-alpine
```

In order to simplify the deployment, we already override some default values of those dependencies:
```YAML
influxdb2:
  enabled: false
  # Additional configuration, See https://github.com/influxdata/helm-charts/blob/master/charts/influxdb2/values.yaml
  livenessProbe:
    path: "/health"
    scheme: "HTTP"
    initialDelaySeconds: 0
    periodSeconds: 10
    timeoutSeconds: 1
    failureThreshold: 3
  
  readinessProbe:
    path: "/health"
    scheme: "HTTP"
    initialDelaySeconds: 0
    periodSeconds: 10
    timeoutSeconds: 1
    successThreshold: 1
    failureThreshold: 3
  
  startupProbe:
    enabled: true
    path: "/health"
    scheme: "HTTP"
    initialDelaySeconds: 30
    periodSeconds: 5
    timeoutSeconds: 1
    failureThreshold: 6
  
telegraf:
  enabled: false
  # Additional configuration, See https://github.com/influxdata/helm-charts/blob/master/charts/telegraf/values.yaml
  config:
    agent:
      interval: "10s"
      round_interval: true
      metric_batch_size: 1000
      metric_buffer_limit: 10000
      collection_jitter: "0s"
      flush_interval: "10s"
      flush_jitter: "0s"
      precision: ""
      debug: true
    inputs:
      - opentelemetry:
          service_address: ":4311" #address to receive traces
          timeout: "5s"
          metrics_schema: "prometheus-v2"
    processors:
      - starlark:
          source:
            |
              load("json.star", "json")
              load("logging.star", "log")
              def apply(metric):
                  log.info("Processing metric: {}".format(metric))
                  if "attributes" in metric.fields:
                      attrs_json = metric.fields["attributes"]
                      attrs = json.decode(attrs_json)
                      # if it is a request move measurment to requests collection
                      if "scope" in attrs and attrs["scope"] == "request":
                          metric.name = "requests"
                          attrs.pop("scope") # remove scope from attributes
                      else:
                          return None #if it is not a request, drop it
                      # copy attributes to tags and drop
                      for k, v in attrs.items():
                          metric.tags[k] = str(v)
                      metric.fields.pop("attributes")
                      # Collect only duration field and drop the rest
                      fields_to_remove = [field for field in metric.fields if field != "duration_nano"]

                      # Drop unwanted fields
                      for field in fields_to_remove:
                        metric.fields.pop(field)
                  else: 
                      return None #if there are no attributes, drop this trace
                  return metric
    outputs:
      - influxdb_v2:
          urls: 
            - "${INFLUXDB_URL}"
          organization: "${INFLUXDB_ORG}"
          bucket: "${INFLUXDB_BUCKET}"
          token: "${INFLUXDB_WRITE_TOKEN}"
          timeout: "5s"

opentelemetry-collector:
  enabled: false
  # Additional configuration. See https://github.com/open-telemetry/opentelemetry-helm-charts/blob/main/charts/opentelemetry-collector/values.yaml
  mode: "deployment"
  resources:
    requests:
      memory: 128Mi
      cpu: 100m
    limits:
      memory: 512Mi
      cpu: 1000m

  image:
    repository: "otel/opentelemetry-collector-contrib"

  config:
    receivers:
      otlp:
        protocols:
          grpc:
            endpoint: ${env:MY_POD_IP}:4317
          http:
            endpoint: ${env:MY_POD_IP}:4318


    exporters:
      otlp/telegraf:
        endpoint: ${env:TELEGRAF_ENDOINT}
        tls:
          insecure: true

    #  otlphttp:
    #    endpoint: http://seq.seq.svc.cluster.local/ingest/otlp

    processors:
      batch: {}
      filter/health: #https://github.com/open-telemetry/opentelemetry-collector-contrib/tree/main/processor/filterprocessor
        error_mode: ignore
        traces:
          span:
            - 'attributes["url.path"] == "/$$liveness"'
            - 'attributes["url.path"] == "/$$readiness"'
      filter/requestmeter:
        spans:
          include:
            match_type: strict
            attributes:
              - key: "scope"
                value: "request"

    service:
      pipelines:
        #traces:
        #  receivers: [otlp]
        #  exporters: [otlphttp]
        #  processors: [filter/health, batch]
        traces/requestmeter:
          receivers: [otlp]
          exporters: [otlp/telegraf]
          processors: [filter/health, filter/requestmeter, batch]

```

The above configuration requires some additional steps to be taken before deploying.
1. Create a secret storing the influxdb2 admin token and password
```SH
kubectl create secret generic <INFLUXDB_ADMIN_SECRET_NAME> --from-literal=admin-token=<INFLUXDB_ADMIN_TOKEN> `
        --from-literal=admin-password=<INFLUXDB_ADMIN_PASSWORD>  --namespace <FSNAMESPACE>
``` 
where:
- `<FSNAMESPACE>`: Firely Server namespace
- `<INFLUXDB_ADMIN_PASSWORD>`: Password to be used for the `admin` user of InfluxDb2
- `<INFLUXDB_ADMIN_TOKEN>`: Token to be used for impersonating the `admin` user in InfluxDb2 API 
- `<INFLUXDB_ADMIN_SECRET_NAME>` : The name of the secret used for storing the Influxdb2 admin credentials

2. Create a secret storing the environment variables required for `telegraf`
```SH
kubectl create secret generic <TELEGRAF_ENV_SECRET_NAME> --namespace <FSNAMESPACE> `
        --from-literal=INFLUXDB_WRITE_TOKEN=<INFLUXDB_ADMIN_TOKEN>
```
- `<TELEGRAF_ENV_SECRET_NAME>` : The name of the secret used for storing confidential environment variables used by Telegraf

Once the above pre-requisite are deployed, you can deploy the Firely Server helm chart as follows:

```SH
$ helm install my-release -n <FSNAMESPACE> -f ./firelyserver-license.yaml --values dependencies.yaml firely-server/firely-server 
```

where the content of `dependencies.yaml` is
```YAML
extraEnvs:
  - name: VONK_RealWorldTesting__InfluxDbOptions__Host
    value: <INFLUXDB_ENDPOINT>
  - name: VONK_RealWorldTesting__InfluxDbOptions__Bucket
    value: <INFLUXDB_RWT_BUCKET>
  - name: VONK_RealWorldTesting__InfluxDbOptions__Organization
    value: <INFLUXDB_RWT_ORG>
  - name: VONK_RealWorldTesting__InfluxDbOptions__Token
    value: <INFLUXDB_ADMIN_TOKEN>
  - name: VONK_OpenTelemetryOptions__Endpoint
    value: <OPENTELEMETRYCOLLECTOR_ENDPOINT>

influxdb2:
  enabled: true
  adminUser:
    existingSecret: <INFLUXDB_ADMIN_SECRET_NAME>
    organization: <INFLUXDB_RWT_ORG>
    bucket: <INFLUXDB_RWT_BUCKET>

telegraf:
  enabled: true
  env:
    - name: INFLUXDB_URL
      value: <INFLUXDB_ENDPOINT>
    - name: INFLUXDB_ORG
      value: <INFLUXDB_RWT_ORG>
    - name: INFLUXDB_BUCKET
      value: <INFLUXDB_RWT_BUCKET>
  envFromSecret: <TELEGRAF_ENV_SECRET_NAME>

opentelemetry-collector:
  enabled: true
  extraEnvs:
    - name: TELEGRAF_ENDOINT
      value: <TELEGRAF_ENDPOINT>
```
where:
- `<OPENTELEMETRYCOLLECTOR_ENDPOINT>`: Opentelemetry-collector internal endpoint, by default `http://my-release--opentelemetry-collector.<FSNAMESPACE>.svc.cluster.local:4317`
- `<INFLUXDB_ENDPOINT>`: Influxdb2 internal endpoint, by default `http://my-release-influxdb2.<FSNAMESPACE>.svc.cluster.local`
- `<INFLUXDB_RWT_ORG>`: The organization owning the bucket where the RWT metrics are stored, for example, `firely`
- `<INFLUXDB_RWT_BUCKET>`: The influxdb2 bucket where the RWT metrics are stored, for exemple, `fs-otel`
- `<TELEGRAF_ENDPOINT>` : Telegraf internal endpoint, by default `http://my-release-telegraf.<FSNAMESPACE>.svc.cluster.local:4311`



