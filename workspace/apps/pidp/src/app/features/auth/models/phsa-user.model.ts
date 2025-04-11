import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class PhsaUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string;
  public lastName: string;
  public email: string;

  public constructor({ accessTokenParsed }: UserIdentity) {
    const { given_name, family_name, email } = accessTokenParsed;
    const {
      identity_provider,
      preferred_username: idpId,
      sub: userId,
    } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.idpId = idpId;
    this.firstName = given_name;
    this.lastName = family_name;
    this.email = email;
  }
}

export class PhsaResolver implements IUserResolver<PhsaUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): PhsaUser {
    return new PhsaUser(this.userIdentity);
  }
}
