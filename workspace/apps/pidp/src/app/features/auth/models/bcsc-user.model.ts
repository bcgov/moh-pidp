import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class BcscUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string;
  public lastName: string;
  public birthdate: string;

  public constructor({ accessTokenParsed }: UserIdentity) {
    const {
      given_name,
      family_name,
      preferred_username: idpId,
      birthdate,
    } = accessTokenParsed;
    const { identity_provider, sub: userId } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.idpId = idpId;
    this.firstName = given_name;
    this.lastName = family_name;
    this.birthdate = birthdate;
  }
}

export class BcscResolver implements IUserResolver<BcscUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): BcscUser {
    return new BcscUser(this.userIdentity);
  }
}
