import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, catchError, map, of } from 'rxjs';
import { PartyService } from './party.service';
import { EndorsementsResource } from '@app/features/organization-info/pages/endorsements/endorsements-resource.service';

@Injectable({
  providedIn: 'root',
})
export class LandingActionsResolver implements Resolve<null> {
  public constructor(
    private partyService: PartyService,
    private router: Router,
    private resource: EndorsementsResource,
  ) {}

  public resolve(route: ActivatedRouteSnapshot): Observable<null> {
    const endorsementToken = route.queryParamMap.get('endorsement-token');
    if (!endorsementToken) {
      return of(null);
    }

    return this.resource
      .receiveEndorsementRequest(this.partyService.partyId, endorsementToken)
      .pipe(
        map(() => {
          this.router.navigate([], {
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
  }
}
