import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { AuthGuardService } from './abstract-auth-guard.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuardService extends AuthGuardService {
  public constructor(
    protected authService: AuthService,
    private router: Router,
  ) {
    super(authService);
  }

  protected handleAccessCheck(
    routeRedirect: string | undefined,
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      authenticated
        ? true
        : this.router.createUrlTree([routeRedirect ?? '/'], {
            queryParams:
              this.router.getCurrentNavigation()?.extractedUrl.queryParams,
            queryParamsHandling: 'merge',
          });
  }

  protected handleAccessError(): boolean {
    return false;
  }
}
