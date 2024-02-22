import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';

import { catchError, exhaustMap, of } from 'rxjs';

import { AuthRoutes } from '@app/features/auth/auth.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { LoggerService } from '../services/logger.service';
import { DiscoveryResource } from './discovery-resource.service';
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
export const partyResolver: ResolveFn<number | null> = () => {
  const router = inject(Router);
  const discoveryResource = inject(DiscoveryResource);
  const partyService = inject(PartyService);
  const logger = inject(LoggerService);

  return discoveryResource.discover().pipe(
    exhaustMap((discovery) => {
      if (discovery.newBCProvider) {
        router.navigateByUrl(
          AuthRoutes.routePath(AuthRoutes.BC_PROVIDER_UPLIFT),
        );
      }
      if (!discovery.partyId) {
        return of(null);
      }

      partyService.partyId = discovery.partyId;
      return of(discovery.partyId);
    }),
    catchError((error: HttpErrorResponse | Error) => {
      logger.error(error.message);

      if (error instanceof HttpErrorResponse && error.status === 403) {
        router.navigateByUrl('denied');
      } else {
        router.navigateByUrl(ShellRoutes.SUPPORT_ERROR_PAGE);
      }

      return of(null);
    }),
  );
};
