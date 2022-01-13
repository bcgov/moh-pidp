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

import { LoggerService } from '@core/services/logger.service';

import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard
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

  private checkAccess(): Observable<boolean | UrlTree> {
    return this.authService.isLoggedIn().pipe(
      map((authenticated: boolean) =>
        authenticated
          ? true
          : this.router.createUrlTree([this.config.routes.auth])
      ),
      catchError((error) => {
        this.logger.error('Error occurred during access validation: ', error);
        return of(false);
      })
    );
  }
}
