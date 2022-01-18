import { Injectable } from '@angular/core';

import { OperatorFunction, map } from 'rxjs';

import {
  AbstractAuthorizedUserService,
  UserIdentity,
} from '@app/features/auth/classes/abstract-authorized-user-service.class';
import { BcscUser } from '@app/features/auth/models/bcsc-user.model';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorizedUserService extends AbstractAuthorizedUserService<BcscUser> {
  public constructor(accessTokenService: AccessTokenService) {
    super(accessTokenService);
  }

  protected fromUserIdentity(): OperatorFunction<
    UserIdentity,
    BcscUser | null
  > {
    return map(({ accessTokenParsed, brokerProfile }: UserIdentity) => {
      if (!accessTokenParsed) {
        return null;
      }

      const { firstName, lastName, birthdate } = brokerProfile;
      const { preferred_username: hpdid, sub: userId } = accessTokenParsed;

      return {
        hpdid,
        userId,
        firstName,
        lastName,
        birthdate,
      };
    });
  }
}
