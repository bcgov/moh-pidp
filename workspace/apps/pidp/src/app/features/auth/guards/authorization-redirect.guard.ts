import { Inject, Injectable } from '@angular/core';
import {
  CanActivate,
  CanActivateChild,
  CanLoad,
  Router,
  UrlTree,
} from '@angular/router';

import { Observable, catchError, map, of } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { LoggerService } from '@app/core/services/logger.service';

import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationRedirectGuard
  implements CanActivate, CanActivateChild, CanLoad
{
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private logger: LoggerService,
    private authService: AuthService
  ) {}

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

  // TODO determine if party or admin and redirect appropriately
  private checkAccess(): Observable<boolean | UrlTree> {
    return this.authService.isLoggedIn().pipe(
      map((authenticated: boolean) =>
        authenticated
          ? this.router.createUrlTree([this.config.routes.portal])
          : true
      ),
      catchError((error) => {
        this.logger.error('Error occurred during access validation: ', error);
        return of(true);
      })
    );
  }
}
