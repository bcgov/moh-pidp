import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

export interface Credential {
  id: number;
  identityProvider: IdentityProvider;
}
