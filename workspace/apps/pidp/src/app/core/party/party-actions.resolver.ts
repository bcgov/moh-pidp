import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';

import { Observable, map, switchMap } from 'rxjs';

import { PartyResolver } from './party.resolver';
import { LandingActionsResolver } from './landing-actions.resolver';

/**
 * @description
 * Combination of the Party + Landing Actions resolvers.
 */
@Injectable({
  providedIn: 'root',
})
export class PartyActionsResolver implements Resolve<number | null> {
  public constructor(
    private partyResolver: PartyResolver,
    private landingActionsResolver: LandingActionsResolver,
  ) {}

  public resolve(route: ActivatedRouteSnapshot): Observable<number | null> {
    return this.partyResolver
      .resolve()
      .pipe(
        switchMap((partyId) =>
          this.landingActionsResolver.resolve(route).pipe(map(() => partyId)),
        ),
      );
  }
}
