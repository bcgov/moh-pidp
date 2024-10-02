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
$ helm dependency build

```

Note: firely/server image requires root privilege to start. However, you can't run the pods as a root user on Openshift.You need to customize the firely/server docker image, create a non-root user and change the ownership of /app folder to the non-root user:

``` console
$ cd ./infra/firely-server 

$ docker build -t firely/server .

$ docker login --username="oc whoami" --password={token} image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools

$ docker tag firely/server image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/firely-server:latest

$ docker push  image-registry.apps.gold.devops.gov.bc.ca/f088b1-tools/firely-server:latest

$ helm upgrade --install --values ./values.yaml firely-server .

```

The command deploys Firely Server on the Kubernetes cluster in the default configuration. The configuration section lists the parameters that can be configured during installation.

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


## Uninstalling the Chart
To uninstall/delete the `my-release` deployment:

``` console 
$ helm delete my-release
```

The command removes all the Kubernetes components associated with the chart and deletes the release.