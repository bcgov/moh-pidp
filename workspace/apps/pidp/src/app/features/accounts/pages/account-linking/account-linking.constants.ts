import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

export const linkedAccountCardText = {
  [IdentityProvider.BCSC]: 'BC Services Card',
  [IdentityProvider.PHSA]: 'Health Authority ID',
  [IdentityProvider.IDIR]: 'IDIR Account',
  [IdentityProvider.BC_PROVIDER]: 'BCProvider Account',
};
