import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class BcProviderApplicationResource {
  public constructor(
    private readonly apiResource: ApiHttpClient,
    private readonly portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public createBcProviderAccount(
    partyId: number,
    password: string,
  ): Observable<string> {
    return this.apiResource
      .post<string>(`${this.getResourcePath(partyId)}/bc-provider`, {
        password,
      })
      .pipe(map((upn) => upn));
  }

  // Currently automatically links to BCProvider
  public createLinkTicket(partyId: number): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.getResourcePath(partyId)}/link-ticket`, {
        linkToIdp: IdentityProvider.BC_PROVIDER,
      })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
