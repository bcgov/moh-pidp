import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateChildFn,
  CanActivateFn,
  CanMatchFn,
  Route,
} from '@angular/router';

import { AuthenticationGuardService } from './services/authentication-guard.service';

export abstract class AuthenticationGuard {
  public static canActivate: CanActivateFn = (
    route: ActivatedRouteSnapshot,
  ) => {
    return inject(AuthenticationGuardService).canActivate(route);
  };

  public static canActivateChild: CanActivateChildFn = (
    childRoute: ActivatedRouteSnapshot,
  ) => {
    return inject(AuthenticationGuardService).canActivateChild(childRoute);
  };

  public static canMatch: CanMatchFn = (route: Route) => {
    return inject(AuthenticationGuardService).canMatch(route);
  };
}
