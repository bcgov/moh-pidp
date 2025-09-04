import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class PhsaUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string | undefined;
  public lastName: string;
  public email: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const { firstName, lastName, email } = brokerProfile;
    const {
      identity_provider,
      preferred_username: idpId,
      sub: userId,
    } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.idpId = idpId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
  }
}

export class PhsaResolver implements IUserResolver<PhsaUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): PhsaUser {
    return new PhsaUser(this.userIdentity);
  }
}
