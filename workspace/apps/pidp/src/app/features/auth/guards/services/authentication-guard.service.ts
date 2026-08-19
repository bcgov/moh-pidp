import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from '../../services/auth.service';
import { AuthGuardService } from './abstract-auth-guard.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuardService extends AuthGuardService {
  protected authService: AuthService;
  private readonly router = inject(Router);

  public constructor() {
    const authService = inject(AuthService);

    super(authService);
  
    this.authService = authService;
  }

  public override canActivate(
    route: ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.isPharmacyEnrolRoute(route)) {
      return true;
    }
    return super.canActivate(route);
  }

  public override canActivateChild(
    childRoute: ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.isPharmacyEnrolRoute(childRoute)) {
      return true;
    }
    return super.canActivateChild(childRoute);
  }

  private isPharmacyEnrolRoute(route: ActivatedRouteSnapshot): boolean {
    let currentRoute: ActivatedRouteSnapshot | null = route;
    while (currentRoute) {
      if (currentRoute.url.some((segment) => segment.path === 'pharmacy-enrol')) {
        return true;
      }
      currentRoute = currentRoute.firstChild;
    }
    return false;
  }

  protected handleAccessCheck(
    routeRedirect: string | undefined,
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      authenticated
        ? true
        : this.router.createUrlTree([routeRedirect ?? '/'], {
            queryParams:
              this.router.currentNavigation()?.extractedUrl.queryParams,
            queryParamsHandling: 'merge',
          });
  }

  protected handleAccessError(): boolean {
    return false;
  }
}
