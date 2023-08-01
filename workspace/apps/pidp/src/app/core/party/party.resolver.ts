import { HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';

import { Observable, catchError, exhaustMap, of, throwError } from 'rxjs';

import { CookieService } from 'ngx-cookie-service';

import { RootRoutes } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { LoggerService } from '../services/logger.service';
import { PartyResource } from './party-resource.service';
import { PartyService } from './party.service';

/**
 * @description
 * Gets a Party from the API based on the access token, and
 * if not found creates the resource before setting the Party
 * identifier in a singleton service.
 *
 * WARNING: Should be located on or under the route config
 * containing guard(s) that manage access, otherwise will
 * redirect to access denied when unauthenticated.
 */
@Injectable({
  providedIn: 'root',
})
export class PartyResolver implements Resolve<number | null> {
  public logoutRedirectUrl: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private partyResource: PartyResource,
    private partyService: PartyService,
    private logger: LoggerService,
    private cookieService: CookieService
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/auth/bc-provider-uplift`;
  }

  public resolve(): Observable<number | null> {
    return this.partyResource.firstOrCreate().pipe(
      exhaustMap((partyId: number | null) =>
        partyId
          ? of((this.partyService.partyId = partyId))
          : throwError(() => new Error('Party could not be found or created'))
      ),
      catchError((error: HttpErrorResponse) => {
        this.logger.error(error.message);

        if (error.status === 403) {
          this.router.navigateByUrl(RootRoutes.DENIED);
        } else if (this.cookieService.get('bcprovider_aad_userid')) {
          // redirect user
          this.router.navigateByUrl('/auth/bc-provider-uplift');
        } else {
          this.router.navigateByUrl(ShellRoutes.SUPPORT_ERROR_PAGE);
        }
        return of(null);
      })
    );
  }
}
