import { KeycloakProfile } from 'keycloak-js';

export interface BrokerProfile extends KeycloakProfile {
  firstName?: string;
  lastName: string;
  birthdate: string;
  username: string;
  email: string;
  attributes: {
    birthdate: string;
  };
}
