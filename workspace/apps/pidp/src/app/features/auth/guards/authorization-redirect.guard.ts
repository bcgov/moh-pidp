import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { AuthService } from '../services/auth.service';
import { AuthGuard } from './abstract-auth.guard';

@Injectable({
  providedIn: 'root',
})
export abstract class AuthorizationRedirectGuard extends AuthGuard {
  public constructor(
    protected authService: AuthService,
    private router: Router
  ) {
    super(authService);
  }

  protected handleAccessCheck(
    routeRedirect: string
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      // Redirect to a route config defined route, or root route and
      // allow the default routing to determine the destination
      authenticated
        ? this.router.createUrlTree([routeRedirect ?? '/'], {
          queryParams: this.router.getCurrentNavigation()?.extractedUrl.queryParams,
          queryParamsHandling: 'merge'
        })
        : true;
  }

  protected handleAccessError(): boolean {
    return true;
  }
}
