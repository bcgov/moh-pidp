import { Injectable } from '@angular/core';

import { Observable, combineLatest, map } from 'rxjs';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { BcscUser } from '../models/bcsc-user.model';
import { IdirUser } from '../models/idir-user.model';
import { PhsaUser } from '../models/phsa-user.model';
import { UserIdentity } from '../models/user-identity.model';
import { User } from '../models/user.model';
import { AccessTokenService } from './access-token.service';

export interface IUserResolver<T extends User> {
  resolve(): T;
}

export class IdirResolver implements IUserResolver<IdirUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): IdirUser {
    return new IdirUser(this.userIdentity);
  }
}

export class BcscResolver implements IUserResolver<BcscUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): BcscUser {
    return new BcscUser(this.userIdentity);
  }
}

export class PhsaResolver implements IUserResolver<PhsaUser> {
  public constructor(public userIdentity: UserIdentity) {}
  public resolve(): PhsaUser {
    return new PhsaUser(this.userIdentity);
  }
}

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

  public get user$(): Observable<User> {
    return this.getUserResolver$().pipe(map((resolver) => resolver.resolve()));
  }

  /**
   * @description
   * Get the authorized user resolver mapped from the access token.
   */
  public getUserResolver$(): Observable<IUserResolver<User>> {
    return combineLatest({
      accessTokenParsed: this.accessTokenService.decodeToken(),
      brokerProfile: this.accessTokenService.loadBrokerProfile(),
    }).pipe(
      map((userIdentity: UserIdentity) => this.getUserResolver(userIdentity))
    );
  }

  private getUserResolver(userIdentity: UserIdentity): IUserResolver<User> {
    switch (userIdentity.accessTokenParsed.identity_provider) {
      case IdentityProvider.IDIR:
        return new IdirResolver(userIdentity);
      case IdentityProvider.BCSC:
        return new BcscResolver(userIdentity);
      case IdentityProvider.PHSA:
        return new PhsaResolver(userIdentity);
      default:
        throw new Error('Identity provider not recognized');
    }
  }
}
