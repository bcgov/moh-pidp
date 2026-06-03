import { Injectable, inject } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { AuthGuardService } from './abstract-auth-guard.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationRedirectGuardService extends AuthGuardService {
  protected authService: AuthService;
  private readonly router = inject(Router);

  public constructor() {
    const authService = inject(AuthService);

    super(authService);
  
    this.authService = authService;
  }

  protected handleAccessCheck(
    routeRedirect: string,
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      // Redirect to a route config defined route, or root route and
      // allow the default routing to determine the destination
      authenticated
        ? this.router.createUrlTree([routeRedirect ?? '/'], {
            queryParams:
              this.router.currentNavigation()?.extractedUrl.queryParams,
            queryParamsHandling: 'merge',
          })
        : true;
  }

  protected handleAccessError(): boolean {
    return true;
  }
}
