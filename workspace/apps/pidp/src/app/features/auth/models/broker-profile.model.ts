import { KeycloakProfile } from 'keycloak-js';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface BrokerProfile extends KeycloakProfile {
  firstName: string;
  lastName: string;
}
