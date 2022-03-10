import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class IdirUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public firstName: string;
  public lastName: string;
  public idir: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const { firstName, lastName } = brokerProfile;
    const {
      identity_provider,
      preferred_username: idir,
      sub: userId,
    } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.idir = idir;
    this.userId = userId;
    this.firstName = firstName;
    this.lastName = lastName;
  }
}

export class IdirResolver implements IUserResolver<IdirUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): IdirUser {
    return new IdirUser(this.userIdentity);
  }
}
