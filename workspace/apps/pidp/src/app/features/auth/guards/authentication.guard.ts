import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { AuthService } from '../services/auth.service';
import { AuthGuard } from './abstract-auth.guard';

@Injectable({
  providedIn: 'root',
})
export abstract class AuthenticationGuard extends AuthGuard {
  public constructor(
    protected authService: AuthService,
    private router: Router
  ) {
    super(authService);
  }

  protected handleAccessCheck(
    routeRedirect: string | undefined
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      authenticated ? true : this.router.createUrlTree([routeRedirect ?? '/']);
  }

  protected handleAccessError(): boolean {
    return false;
  }
}
