import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class InfantRsvEformsResource {
  private readonly apiResource = inject(ApiHttpClient);
  private readonly portalResource = inject(PortalResource);

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(partyId: number): NoContent {
    return this.apiResource
      .post<NoContent>(
        `parties/${partyId}/access-requests/infant-rsv-eforms`,
        {},
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }
}
