import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';

import { Observable, catchError, exhaustMap, of } from 'rxjs';

import { RootRoutes } from '@bcgov/shared/ui';

import { AuthRoutes } from '@app/features/auth/auth.routes';
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
  public constructor(
    private router: Router,
    private partyResource: PartyResource,
    private partyService: PartyService,
    private logger: LoggerService
  ) {}

  public resolve(): Observable<number | null> {
    return this.partyResource.firstOrCreate().pipe(
      exhaustMap((response: number) =>
        of((this.partyService.partyId = response))
      ),
      catchError((error: HttpErrorResponse | Error) => {
        this.logger.error(error.message);

        if (error instanceof HttpErrorResponse && error.status == 409) {
          // User is an unlinked BC Provider.
          this.router.navigateByUrl(
            AuthRoutes.routePath(AuthRoutes.BC_PROVIDER_UPLIFT)
          );
        } else if (error instanceof HttpErrorResponse && error.status === 403) {
          this.router.navigateByUrl(RootRoutes.DENIED);
        } else {
          this.router.navigateByUrl(ShellRoutes.SUPPORT_ERROR_PAGE);
        }

        return of(null);
      })
    );
  }
}
