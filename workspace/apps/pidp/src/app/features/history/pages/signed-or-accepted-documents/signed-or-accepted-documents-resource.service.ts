import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';

import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class SignedOrAcceptedDocumentsResource {
  private portalResource = inject(PortalResource);


  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }
}
