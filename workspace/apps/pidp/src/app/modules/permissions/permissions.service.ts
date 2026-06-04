import { Injectable, inject } from '@angular/core';

import { AccessTokenService } from '@app/features/auth/services/access-token.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionsService {
  private readonly accessTokenService = inject(AccessTokenService);


  public hasRole(allowedRoles: string | string[]): boolean {
    allowedRoles = Array.isArray(allowedRoles) ? allowedRoles : [allowedRoles];
    return this.accessTokenService
      .roles()
      .some((role) => allowedRoles.includes(role));
  }
}
