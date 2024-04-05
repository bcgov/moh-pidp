import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, tap, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { EndorsementRequestInformation } from './models/endorsement-request-information.model';
import { EndorsementRequest } from './models/endorsement-request.model';
import { Endorsement } from './models/endorsement.model';

@Injectable({
  providedIn: 'root',
})
export class EndorsementsResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private toastService: ToastService,
  ) {}

  public getEndorsements(partyId: number): Observable<Endorsement[] | null> {
    return this.apiResource
      .get<Endorsement[]>(this.getEndorsementsResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        }),
      );
  }

  public cancelEndorsement(partyId: number, endorsementId: number): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getEndorsementsResourcePath(partyId)}/${endorsementId}/cancel`,
        null,
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        }),
      );
  }

  public getEndorsementRequests(
    partyId: number,
  ): Observable<EndorsementRequest[] | null> {
    return this.apiResource
      .get<EndorsementRequest[]>(this.getRequestsResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        }),
      );
  }

  public createEndorsementRequest(
    partyId: number,
    endorsementRequest: EndorsementRequestInformation,
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        this.getRequestsResourcePath(partyId),
        endorsementRequest,
      )
      .pipe(
        NoContentResponse,
        tap(() =>
          this.toastService.openSuccessToast(
            'Endorsement Request has been created',
          ),
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.UnprocessableEntity) {
            this.toastService.openErrorToast(
              'You already have an active endorsement with this email address',
            );
          }

          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        }),
      );
  }

  public receiveEndorsementRequest(partyId: number, token: string): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.getRequestsResourcePath(partyId)}/receive`, {
        token,
      })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        }),
      );
  }

  public approveEndorsementRequest(
    partyId: number,
    endorsementRequestId: number,
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getRequestByIdResourcePath(
          partyId,
          endorsementRequestId,
        )}/approve`,
        null,
      )
      .pipe(
        NoContentResponse,
        tap(() =>
        this.toastService.openSuccessToast(
          'Endorsement Request has been approved successfully',
        ),
      ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        }),
      );
  }

  public declineEndorsementRequest(
    partyId: number,
    endorsementRequestId: number,
  ): NoContent {
    return this.apiResource
      .post<NoContent>(
        `${this.getRequestByIdResourcePath(
          partyId,
          endorsementRequestId,
        )}/decline`,
        null,
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        }),
      );
  }

  private getEndorsementsResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsements`;
  }

  private getRequestsResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsement-requests`;
  }

  private getRequestByIdResourcePath(
    partyId: number,
    endorsementRequestId: number,
  ): string {
    return `${this.getRequestsResourcePath(partyId)}/${endorsementRequestId}`;
  }
}
