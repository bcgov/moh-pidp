import { Injectable } from '@angular/core';

import { OperatorFunction, map } from 'rxjs';

import {
  AbstractAuthorizedUserService,
  UserIdentity,
} from '@app/features/auth/classes/abstract-authorized-user-service.class';
import { Admin } from '@app/features/auth/models/admin.model';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorizedAdminService extends AbstractAuthorizedUserService<Admin> {
  public constructor(accessTokenService: AccessTokenService) {
    super(accessTokenService);
  }

  protected fromUserIdentity(): OperatorFunction<UserIdentity, Admin | null> {
    return map(({ accessTokenParsed, brokerProfile }: UserIdentity) => {
      if (!accessTokenParsed) {
        return null;
      }

      const { firstName, lastName } = brokerProfile;
      const { preferred_username: idir, sub: userId } = accessTokenParsed;

      return {
        idir,
        userId,
        firstName,
        lastName,
      };
    });
  }
}
