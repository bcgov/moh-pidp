import { IdentityProvider } from '../enums/identity-provider.enum';
import { UserIdentity } from './user-identity.model';
import { IUserResolver, User } from './user.model';

export class BcscUser implements User {
  public readonly identityProvider: IdentityProvider;
  public userId: string;
  public idpId: string;
  public firstName: string | undefined;
  public lastName: string;
  public birthdate: string;

  public constructor({ accessTokenParsed, brokerProfile }: UserIdentity) {
    const {
      firstName,
      lastName,
      username: idpId,
      attributes: {
        birthdate: [birthdate],
      },
    } = brokerProfile;
    const { identity_provider, sub: userId } = accessTokenParsed;

    this.identityProvider = identity_provider;
    this.userId = userId;
    this.idpId = idpId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.birthdate = birthdate;
  }
}

export class BcscResolver implements IUserResolver<BcscUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): BcscUser {
    return new BcscUser(this.userIdentity);
  }
}
