import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class ProvincialAttachmentSystemResource {
  public constructor(private readonly portalResource: PortalResource) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }
}
