import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable, catchError, delay, map, of, throwError } from 'rxjs';

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
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  public verifyEmail(partyId: number, token: string): Observable<NoContent> {
    return this.apiResource
      .post<NoContent>(
        `${this.getResourcePath(partyId)}/verified-emails/verify`,
        {
          token,
        },
      )
      .pipe(
        map(() => ({}) as NoContent),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  public createExternalAccount(
    partyId: number,
    userPrincipalName: string,
  ): Observable<NoContent> {
    return this.apiResource
      .post<NoContent>(
        `${this.getResourcePath(partyId)}/credentials/bc-provider/invite`,
        {
          userPrincipalName,
        },
      )
      .pipe(
        map(() => ({}) as NoContent),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}`;
  }
}
