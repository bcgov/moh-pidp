import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn } from '@angular/router';

import { AuthorizationRedirectGuardService } from './services/authorization-redirect-guard.service';

export const authorizationRedirectGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
) => {
  return inject(AuthorizationRedirectGuardService).canActivate(route);
};
