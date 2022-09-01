import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { EndorsementRequestInformation } from './models/endorsement-request-information.model';
import { EndorsementRequest } from './models/endorsement-request.model';

@Injectable({
  providedIn: 'root',
})
export class EndorsementsResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public getEndorsementRequests(
    partyId: number
  ): Observable<EndorsementRequest[] | null> {
    return this.apiResource
      .get<EndorsementRequest[]>(
        this.getEndorsementRequestsResourcePath(partyId)
      )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        })
      );
  }

  public createEndorsementRequest(
    partyId: number,
    endorsementRequest: EndorsementRequestInformation
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        this.getEndorsementRequestsResourcePath(partyId),
        endorsementRequest
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

  public receiveEndorsementRequest(partyId: number, token: string): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getEndorsementRequestsResourcePath(partyId)}/receive`,
        { token }
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

  public approveEndorsementRequest(
    partyId: number,
    endorsementRequestId: number
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getEndorsementRequestResourcePath(
          partyId,
          endorsementRequestId
        )}/approve`,
        null
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

  public declineEndorsementRequest(
    partyId: number,
    endorsementRequestId: number
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getEndorsementRequestResourcePath(
          partyId,
          endorsementRequestId
        )}/decline`,
        null
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

  private getEndorsementsResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsements`;
  }

  private getEndorsementRequestsResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsement-requests`;
  }

  private getEndorsementRequestResourcePath(
    partyId: number,
    endorsementRequestId: number
  ): string {
    return `${this.getEndorsementRequestsResourcePath(
      partyId
    )}/${endorsementRequestId}`;
  }
}
