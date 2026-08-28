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
    return super.canActivate(route);
  }

  public override canActivateChild(
    childRoute: ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return super.canActivateChild(childRoute);
  }

  protected handleAccessCheck(
    routeRedirect: string | undefined,
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree => {
      if (authenticated) {
        return true;
      }

      const currentNav = this.router.getCurrentNavigation();
      const currentUrl = currentNav?.extractedUrl.toString() || this.router.url;
      const queryParams: Record<string, string> = {
        ...(currentNav?.extractedUrl.queryParams || {}),
      };

      if (currentUrl !== '/' && !currentUrl.startsWith('/?')) {
        queryParams['return-url'] = currentUrl;
      }

      return this.router.createUrlTree([routeRedirect ?? '/'], {
        queryParams,
        queryParamsHandling: 'merge',
      });
    };
  }

  protected handleAccessError(): boolean {
    return false;
  }
}
