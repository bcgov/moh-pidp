import {
  CanActivate,
  CanActivateChild,
  CanLoad,
  UrlTree,
} from '@angular/router';

import { Observable, catchError, map, of } from 'rxjs';

import { AuthService } from '../services/auth.service';

export abstract class AuthGuard
  implements CanActivate, CanActivateChild, CanLoad
{
  public constructor(protected authService: AuthService) {}

  public canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess();
  }

  public canActivateChild():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess();
  }

  public canLoad():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.checkAccess();
  }

  protected abstract getRedirectUrl(): string;

  protected checkAccess(): Observable<boolean | UrlTree> {
    return this.authService.isLoggedIn().pipe(
      map(this.handleAccessCheck()),
      catchError((error) => {
        console.error('Error occurred during access validation: ', error);
        return of(this.handleAccessError());
      })
    );
  }

  /**
   * @description
   * Hook for checking whether the user is authenticated and
   * should be provided access or redirected.
   */
  protected abstract handleAccessCheck(): (
    authenticated: boolean
  ) => boolean | UrlTree;

  /**
   * @description
   * Hook for determining access or not based on authentication
   * failing for an unknown reason.
   */
  protected abstract handleAccessError(): boolean;
}
