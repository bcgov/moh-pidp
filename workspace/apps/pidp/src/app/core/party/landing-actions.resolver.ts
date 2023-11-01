import { Injectable } from '@angular/core';
import {
  Router,
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
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

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean> {
    return this.resource.getProfileStatus(this.partyService.partyId).pipe(
      switchMap((result) => {
        console.log(' ---------- done ---------');
        return of(true);
      }),
    );
  }
}
