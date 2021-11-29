import { KeycloakOptions } from 'keycloak-angular';

import { environmentName } from './environment.model';

export interface ConfigMap {
  environmentName: environmentName;
  apiEndpoint: string;
  keycloakConfig: KeycloakOptions;
}
