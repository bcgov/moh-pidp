import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  Router,
  UrlTree,
} from '@angular/router';

import { Observable } from 'rxjs';

import { PermissionsService } from './permissions.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionsGuard
  implements CanActivate, CanActivateChild, CanLoad
{
  public constructor(
    private router: Router,
    private permissionsService: PermissionsService
  ) {}

  public canActivate(
    route: ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkPermissions(route);
  }

  public canActivateChild(
    childRoute: ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkPermissions(childRoute);
  }

  public canLoad(
    route: Route
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkPermissions(route);
  }

  private checkPermissions(
    route: Route | ActivatedRouteSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return (
      this.permissionsService.hasRole(route.data?.roles ?? []) ||
      this.router.createUrlTree(['/'])
    );
  }
}
