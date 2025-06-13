import { ActivatedRouteSnapshot, Route, UrlTree } from '@angular/router';

import { Observable, catchError, map, of } from 'rxjs';

import { AuthService } from '../../services/auth.service';

export abstract class AuthGuardService {
  public constructor(protected authService: AuthService) {}

  public canActivate(
    route: ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess(route.data?.routes?.auth);
  }

  public canActivateChild(
    childRoute: ActivatedRouteSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess(childRoute.data?.routes?.auth);
  }

  public canMatch(
    route: Route,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess(route.data?.routes?.auth);
  }

  protected checkAccess(
    routeRedirect: string | undefined,
  ): Observable<boolean | UrlTree> {
    return this.authService.isLoggedIn().pipe(
      // TODO Is this correct to map to false?
      map((authenticated) => authenticated ?? false),
      map(this.handleAccessCheck(routeRedirect)),
      catchError((error) => {
        console.error('Error occurred during access validation: ', error);
        return of(this.handleAccessError());
      }),
    );
  }

  /**
   * @description
   * Hook for checking whether the user is authenticated and
   * should be provided access or redirected.
   */
  protected abstract handleAccessCheck(
    routeRedirect: string | undefined,
  ): (authenticated: boolean) => boolean | UrlTree;

  /**
   * @description
   * Hook for determining access or not based on authentication
   * failing for an unknown reason.
   */
  protected abstract handleAccessError(): boolean;
}
