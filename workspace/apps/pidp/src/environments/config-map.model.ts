import { KeycloakOptions } from 'keycloak-angular';

import { environmentName } from './environment.model';

// TODO rename ConfigMap
export interface ConfigMap {
  environmentName: environmentName;
  apiEndpoint: string;
  keycloakConfig: KeycloakOptions;
}
