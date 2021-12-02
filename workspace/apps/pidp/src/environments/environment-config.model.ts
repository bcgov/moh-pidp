import { KeycloakOptions } from 'keycloak-angular';

import { environmentName } from './environment.model';

export interface EnvironmentConfig {
  environmentName: environmentName;
  apiEndpoint: string;
  keycloakConfig: KeycloakOptions;
}
