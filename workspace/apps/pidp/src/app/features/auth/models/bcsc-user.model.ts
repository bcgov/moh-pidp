import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { User } from './user.model';

export class BcscUser implements User {
  public readonly identityProvider: IdentityProvider;
  public hpdid: string;
  public userId: string;
  public firstName: string;
  public lastName: string;
  public birthdate: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const {
      firstName,
      lastName,
      username: hpdid,
      attributes: {
        birthdate: [birthdate],
      },
    } = brokerProfile;
    const { identity_provider, sub: userId } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.hpdid = hpdid;
    this.userId = userId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.birthdate = birthdate;
  }
}
