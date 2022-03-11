import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';

import { Observable, catchError, exhaustMap, of, throwError } from 'rxjs';

import { RootRoutes } from '@bcgov/shared/ui';

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
      exhaustMap((partyId: number | null) =>
        partyId
          ? of((this.partyService.partyId = partyId))
          : throwError(() => new Error('Party could not be found or created'))
      ),
      catchError((error: Error) => {
        this.logger.error(error.message);
        this.router.navigateByUrl(RootRoutes.DENIED);
        return of(null);
      })
    );
  }
}
