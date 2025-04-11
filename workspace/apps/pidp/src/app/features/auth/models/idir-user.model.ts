import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class IdirUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string;
  public lastName: string;

  public constructor({ accessTokenParsed }: UserIdentity) {
    const { given_name, family_name } = accessTokenParsed;
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
  }
}

export class IdirResolver implements IUserResolver<IdirUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): IdirUser {
    return new IdirUser(this.userIdentity);
  }
}
