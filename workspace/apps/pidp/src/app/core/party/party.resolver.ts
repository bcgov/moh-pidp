import { Portal } from '@angular/cdk/portal';
import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';

import { Observable, catchError, exhaustMap, of } from 'rxjs';

import { RootRoutes } from '@bcgov/shared/ui';

import { AuthRoutes } from '@app/features/auth/auth.routes';
import { PortalRoutes } from '@app/features/portal/portal.routes';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { LoggerService } from '../services/logger.service';
import { Destination, PartyResource } from './party-resource.service';
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
    return this.partyResource.discover().pipe(
      exhaustMap((discovery) => {
        switch (discovery.destination) {
          case Destination.NEW_BC_PROVIDER:
            this.logger.info('BCP');
            this.router.navigateByUrl(
              AuthRoutes.routePath(AuthRoutes.BC_PROVIDER_UPLIFT)
            );
            break;
          case Destination.DEMOGRAPHICS:
            this.logger.info('demo');
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO)
            );
            break;
          case Destination.USER_ACCESS_AGREEMENT:
            this.logger.info('uaa');
            this.router.navigateByUrl(
              ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT)
            );
            break;
          case Destination.PORTAL:
            this.logger.info('portal');
            this.router.navigateByUrl(PortalRoutes.MODULE_PATH);
            break;
          default:
            this.logger.error(
              `Unable to resolve destination '${discovery.destination}'.`
            );
            this.router.navigateByUrl(ShellRoutes.SUPPORT_ERROR_PAGE);
        }

        return discovery.partyId
          ? of((this.partyService.partyId = discovery.partyId))
          : of(null);
      }),
      catchError((error: HttpErrorResponse | Error) => {
        this.logger.error(error.message);

        if (error instanceof HttpErrorResponse && error.status === 403) {
          this.router.navigateByUrl(RootRoutes.DENIED);
        } else {
          this.router.navigateByUrl(ShellRoutes.SUPPORT_ERROR_PAGE);
        }

        return of(null);
      })
    );
  }
}
