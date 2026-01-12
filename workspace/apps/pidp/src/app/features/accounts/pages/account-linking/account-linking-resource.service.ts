import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { Credential } from './account-linking.model';

@Injectable({
  providedIn: 'root',
})
export class AccountLinkingResource {
  public constructor(
    protected apiResource: ApiHttpClient,
    private portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public createLinkTicket(
    partyId: number,
    linkToIdp: IdentityProvider,
  ): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.getResourcePath(partyId)}/link-ticket`, {
        linkToIdp,
      })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  public getCredentials(partyId: number): Observable<Credential[]> {
    return this.apiResource
      .get<Credential[]>(this.getResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
