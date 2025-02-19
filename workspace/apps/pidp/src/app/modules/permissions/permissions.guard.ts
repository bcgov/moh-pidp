import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateChildFn,
  CanActivateFn,
  CanMatchFn,
  Route,
  Router,
  UrlTree,
} from '@angular/router';

import { Observable } from 'rxjs';

import { PermissionsService } from './permissions.service';

export abstract class PermissionsGuard {
  public static readonly canActivate: CanActivateFn = (
    route: ActivatedRouteSnapshot,
  ) => {
    return this.checkPermissions(route);
  };

  public static readonly canActivateChild: CanActivateChildFn = (
    childRoute: ActivatedRouteSnapshot,
  ) => {
    return this.checkPermissions(childRoute);
  };

  public static readonly canMatch: CanMatchFn = (route: Route) => {
    return this.checkPermissions(route);
  };

  private static checkPermissions(
    route: Route | ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const router = inject(Router);
    const permissionsService = inject(PermissionsService);
    return (
      permissionsService.hasRole(route.data?.roles ?? []) ||
      router.createUrlTree(['/'])
    );
  }
}
