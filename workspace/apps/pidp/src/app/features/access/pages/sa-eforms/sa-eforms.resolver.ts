import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';

import { Observable, catchError, map, of } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';

import { SaEformsResource } from './sa-eforms-resource.service';

@Injectable({
  providedIn: 'root',
})
export class SaEformsResolver implements Resolve<boolean> {
  public constructor(
    private saEformsResource: SaEformsResource,
    private partyService: PartyService
  ) {}

  public resolve(): Observable<boolean> {
    if (!this.partyService.partyId) {
      return of(false);
    }

    return this.saEformsResource.requestAccess(this.partyService.partyId).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }
}
