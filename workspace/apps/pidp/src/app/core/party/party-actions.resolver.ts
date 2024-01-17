import { Injector, inject, runInInjectionContext } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  ResolveFn,
  RouterStateSnapshot,
} from '@angular/router';

import { Observable, map, switchMap } from 'rxjs';

import { landingActionsResolver } from './landing-actions.resolver';
import { partyResolver } from './party.resolver';

/**
 * @description
 * Combination of the Party + Landing Actions resolvers.
 */
export const partyActionsResolver: ResolveFn<number | null> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const injector = inject(Injector);
  return (partyResolver(route, state) as Observable<number | null>).pipe(
    switchMap((partyId) => {
      return runInInjectionContext(injector, () => {
        return (landingActionsResolver(route, state) as Observable<null>).pipe(
          map(() => partyId),
        );
      });
    }),
  );
};
