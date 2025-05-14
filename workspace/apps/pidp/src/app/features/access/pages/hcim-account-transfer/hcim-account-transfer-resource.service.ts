import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, of, throwError } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { HcimAccountTransfer } from './hcim-account-transfer.model';

export enum HcimAccountTransferStatusCode {
  ACCESS_GRANTED,
  AUTHENTICATION_FAILED,
  ACCOUNT_LOCKED,
  ACCOUNT_UNAUTHORIZED,
}

export interface HcimAccountTransferResponse {
  statusCode: HcimAccountTransferStatusCode;
  remainingAttempts?: number;
}

@Injectable({
  providedIn: 'root',
})
export class HcimAccountTransferResource {
  public constructor(
    protected apiResource: ApiHttpClient,
    private readonly portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(
    partyId: number,
    ldapCredentials: HcimAccountTransfer,
  ): Observable<HcimAccountTransferResponse> {
    return this.apiResource
      .post<NoContent>(
        `parties/${partyId}/access-requests/hcim-account-transfer`,
        ldapCredentials,
      )
      .pipe(
        map(() => ({
          statusCode: HcimAccountTransferStatusCode.ACCESS_GRANTED,
        })),
        catchError((error: HttpErrorResponse) => {
          switch (error.status) {
            case HttpStatusCode.UnprocessableEntity:
              return of({
                statusCode: HcimAccountTransferStatusCode.AUTHENTICATION_FAILED,
                remainingAttempts: error.headers.get('Remaining-Attempts'),
              });
            case HttpStatusCode.Locked:
              return of({
                statusCode: HcimAccountTransferStatusCode.ACCOUNT_LOCKED,
              });
            case HttpStatusCode.Forbidden:
              return of({
                statusCode: HcimAccountTransferStatusCode.ACCOUNT_UNAUTHORIZED,
              });
          }

          return throwError(() => error);
        }),
      );
  }
}
