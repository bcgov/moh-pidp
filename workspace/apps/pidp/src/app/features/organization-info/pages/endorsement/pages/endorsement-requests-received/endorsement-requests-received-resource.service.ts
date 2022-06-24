import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { ReceivedEndorsementRequest } from '../../models/received-endorsement-request';

@Injectable({
  providedIn: 'root',
})
export class EndorsementRequestsReceivedResource extends CrudResource<ReceivedEndorsementRequest> {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource
  ) {
    super(apiResource);
  }

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public receiveEndorsementRequest(partyId: number, token: string): NoContent {
    return this.apiResource
      .post<NoContent>(this.getResourcePath(partyId), { token })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        })
      );
  }

  public getReceivedEndorsementRequests(
    partyId: number
  ): Observable<ReceivedEndorsementRequest[]> {
    return this.apiResource
      .get<ReceivedEndorsementRequest[]>(this.getResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of([]);
          }

          return throwError(() => error);
        })
      );
  }

  public adjudicateEndorsementRequest(
    partyId: number,
    endorsementRequestId: number,
    approved: boolean
  ): NoContent {
    return this.apiResource
      .put<NoContent>(
        `${this.getResourcePath(partyId)}/${endorsementRequestId}/adjudicate`,
        { approved }
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        })
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsement-requests/received`;
  }
}
