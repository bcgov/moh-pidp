import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, throwError } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class ExternalAccountsResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public createExternalAccount(
    partyId: number,
    userPrincipalName: string,
  ): Observable<NoContent> {
    return this.apiResource
      .post<NoContent>(`${this.getResourcePath(partyId)}/bc-provider/invite`, {
        userPrincipalName,
      })
      .pipe(
        map(() => ({}) as NoContent),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
