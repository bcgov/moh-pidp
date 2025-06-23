import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';

import { Observable, catchError, map, throwError } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

@Injectable({
  providedIn: 'root',
})
export class ExternalAccountsResource {
  public currentStep = signal(1);

  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public checkIfEmailIsVerified(
    partyId: number,
    userPrincipalName: string,
  ): Observable<NoContent> {
    return this.apiResource
      .post<NoContent>(`${this.getResourcePath(partyId)}/verified-emails`, {
        email: userPrincipalName,
      })
      .pipe(
        map(() => ({}) as NoContent),
        catchError((error: HttpErrorResponse) => throwError(() => error)),
      );
  }

  public verifyEmail(
    partyId: number,
    token: string,
  ): Observable<{ email: string }> {
    return this.apiResource
      .post<{ email: string }>(
        `${this.getResourcePath(partyId)}/verified-emails/verify`,
        { token },
      )
      .pipe(catchError((error: HttpErrorResponse) => throwError(() => error)));
  }

  public createExternalAccount(
    partyId: number,
    email: string,
  ): Observable<NoContent> {
    return this.apiResource
      .post<NoContent>(
        `${this.getResourcePath(partyId)}/credentials/bc-provider/invite`,
        { email },
      )
      .pipe(
        map(() => ({}) as NoContent),
        catchError((error: HttpErrorResponse) => throwError(() => error)),
      );
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}`;
  }
}
