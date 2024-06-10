import { Injectable } from '@angular/core';

import { Observable, combineLatest, map } from 'rxjs';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { BcProviderResolver } from '../models/bc-provider-user.model';
import { BcscResolver } from '../models/bcsc-user.model';
import { IdirResolver } from '../models/idir-user.model';
import { PhsaResolver } from '../models/phsa-user.model';
import { UserIdentity } from '../models/user-identity.model';
import { IUserResolver, User } from '../models/user.model';
import { AccessTokenService } from './access-token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorizedUserService {
  public constructor(private accessTokenService: AccessTokenService) {}

  /**
   * @description
   * Get the user roles from the access token.
   */
  public get roles(): string[] {
    return this.accessTokenService.roles();
  }

  /**
   * @description
   * Get the authenticated user subtype based on
   * identity provider.
   */
  public get user$(): Observable<User> {
    return this.getUserResolver$().pipe(map((resolver) => resolver.resolve()));
  }

  /**
   * @description
   * Get the identity provider used to authenticate.
   */
  public get identityProvider$(): Observable<IdentityProvider> {
    return this.user$.pipe(map((user: User) => user.identityProvider));
  }

  /**
   * @description
   * Get the authorized user resolver mapped from
   * the access token.
   */
  private getUserResolver$(): Observable<IUserResolver<User>> {
    return combineLatest({
      accessTokenParsed: this.accessTokenService.decodeToken(),
      brokerProfile: this.accessTokenService.loadBrokerProfile(),
    }).pipe(
      map((userIdentity: UserIdentity) => this.getUserResolver(userIdentity)),
    );
  }

  /**
   * @description
   * Factory for generating an user subtype based
   * on identity provider.
   */
  private getUserResolver(userIdentity: UserIdentity): IUserResolver<User> {
    switch (userIdentity.accessTokenParsed.identity_provider) {
      case IdentityProvider.IDIR:
        return new IdirResolver(userIdentity);
      case IdentityProvider.BCSC:
        return new BcscResolver(userIdentity);
      case IdentityProvider.BC_PROVIDER:
        return new BcProviderResolver(userIdentity);
      case IdentityProvider.PHSA:
        return new PhsaResolver(userIdentity);
      default:
        throw new Error('Identity provider not recognized');
    }
  }
}
