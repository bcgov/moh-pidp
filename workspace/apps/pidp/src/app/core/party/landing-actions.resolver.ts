import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { Observable, of, switchMap } from 'rxjs';
import { PartyService } from './party.service';

@Injectable({
  providedIn: 'root',
})
export class LandingActionsResolver implements Resolve<boolean> {
  public constructor(
    private partyService: PartyService,
    private resource: PortalResource,
  ) {}

  public resolve(): Observable<boolean> {
    return this.resource.getProfileStatus(this.partyService.partyId).pipe(
      switchMap(() => {
        console.log(' ---------- done ---------');
        return of(true);
      }),
    );
  }
}
