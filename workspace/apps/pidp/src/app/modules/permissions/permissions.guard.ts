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

const checkPermissions = (
  route: Route | ActivatedRouteSnapshot
):
  | Observable<boolean | UrlTree>
  | Promise<boolean | UrlTree>
  | boolean
  | UrlTree => {
  const router = inject(Router);
  const permissionsService = inject(PermissionsService);
  return (
    permissionsService.hasRole(route.data?.roles ?? []) ||
    router.createUrlTree(['/'])
  );
};

export const canActivatePermissionsGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {
  return checkPermissions(route);
};

export const canActivateChildPermissionsGuard: CanActivateChildFn = (
  childRoute: ActivatedRouteSnapshot
) => {
  return checkPermissions(childRoute);
};

export const canMatchPermissionsGuard: CanMatchFn = (route: Route) => {
  return checkPermissions(route);
};
