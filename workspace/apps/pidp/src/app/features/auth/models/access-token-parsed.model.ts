import { KeycloakTokenParsed } from 'keycloak-js';

export interface AccessTokenParsed extends KeycloakTokenParsed {
  acr: string;
  'allowed-origins': string[];
  aud: string[];
  auth_time: number;
  azp: string;
  iss: string;
  jti: string;
  scope: string;
  typ: string;
  // User specific attributes:
  email_verified: boolean;
  family_name: string;
  given_name: string;
  name: string;
  // Contains identity provider: example@bcsc or @idir
  preferred_username: string;
}
