import { Injectable } from '@angular/core';

import { Observable, map } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class BcProviderApplicationResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public createBcProviderAccount(
    partyId: number,
    password: string
  ): Observable<string> {
    return this.apiResource
      .post<string>(`${this.getResourcePath(partyId)}/bc-provider`, {
        password,
      })
      .pipe(map((upn) => upn));
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
