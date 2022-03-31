import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { PortalResource } from '@app/features/portal/portal-resource.service';
import { ProfileStatus } from '@app/features/portal/sections/models/profile-status.model';

@Injectable({
  providedIn: 'root',
})
export class SignedOrAcceptedDocumentsResource {
  public constructor(private portalResource: PortalResource) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }
}
