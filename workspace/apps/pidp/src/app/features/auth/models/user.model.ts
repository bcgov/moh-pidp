import { User as BaseUser } from '@bcgov/shared/data-access';

import { KeycloakUser } from '@app/modules/keycloak/keycloak-user';

import { IdentityProvider } from '../enums/identity-provider.enum';

export interface User extends BaseUser, KeycloakUser<IdentityProvider> {
  // Optional properties applied to the User model
  // in an attempt to normalize between users
  email?: string;
  birthdate?: string;
}

export interface IUserResolver<T extends User> {
  resolve(): T;
}
