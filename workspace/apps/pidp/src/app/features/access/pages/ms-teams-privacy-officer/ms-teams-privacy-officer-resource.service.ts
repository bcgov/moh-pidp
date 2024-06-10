import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { MsTeamsClinicInfo } from './ms-teams-privacy-officer.model';

@Injectable({
  providedIn: 'root',
})
export class MsTeamsPrivacyOfficerResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(
    partyId: number,
    clinicInfo: MsTeamsClinicInfo,
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        `parties/${partyId}/access-requests/ms-teams-privacy-officer`,
        clinicInfo,
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }
}
