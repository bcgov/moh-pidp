import {
  ActivatedRouteSnapshot,
  ResolveFn,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable, map, switchMap } from 'rxjs';

import { partyResolver } from './party.resolver';
import { landingActionsResolver } from './landing-actions.resolver';

/**
 * @description
 * Combination of the Party + Landing Actions resolvers.
 */
export const partyActionsResolver: ResolveFn<number | null> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  return (partyResolver(route, state) as Observable<number | null>).pipe(
    switchMap((partyId) =>
      (landingActionsResolver(route, state) as Observable<number | null>).pipe(
        map(() => partyId),
      ),
    ),
  );
};
