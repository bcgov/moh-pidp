import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class BcProviderUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string | undefined;
  public lastName: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const { firstName, lastName, username: idpId } = brokerProfile;
    const { identity_provider, sub: userId } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.idpId = idpId;
    this.firstName = firstName;
    this.lastName = lastName;
  }
}

export class BcProviderResolver implements IUserResolver<BcProviderUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): BcProviderUser {
    return new BcProviderUser(this.userIdentity);
  }
}
