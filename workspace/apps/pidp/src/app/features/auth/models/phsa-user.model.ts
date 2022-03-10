import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { User } from './user.model';

export class PhsaUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public firstName: string;
  public lastName: string;
  public email: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const { firstName, lastName, email } = brokerProfile;
    const { identity_provider, sub: userId } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
  }
}
