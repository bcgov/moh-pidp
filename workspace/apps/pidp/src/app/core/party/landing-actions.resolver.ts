import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Router } from '@angular/router';

import { catchError, map, of } from 'rxjs';

import { EndorsementsResource } from '@app/features/organization-info/pages/endorsements/endorsements-resource.service';

import { PartyService } from './party.service';

export const landingActionsResolver: ResolveFn<null> = (
  route: ActivatedRouteSnapshot,
) => {
  const partyService = inject(PartyService);
  const resource = inject(EndorsementsResource);
  const router = inject(Router);

  const endorsementToken = route.queryParamMap.get('endorsement-token');
  if (!endorsementToken) {
    return of(null);
  }

  return resource
    .receiveEndorsementRequest(partyService.partyId, endorsementToken)
    .pipe(
      map(() => {
        router.navigate([], {
          queryParams: {
            'endorsement-token': null,
          },
          queryParamsHandling: 'merge',
        });
        return null;
      }),
      catchError((error) => {
        console.error(
          'Error occurred when receiving Endorsement Request: ',
          error,
        );
        return of(null);
      }),
    );
};
