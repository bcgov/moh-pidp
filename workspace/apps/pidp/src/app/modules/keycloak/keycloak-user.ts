export interface KeycloakUser<T> {
  userId: string;
  identityProvider: T;
}
