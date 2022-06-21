import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, Observable, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ReceivedEndorsementRequest } from '../../models/received-endorsement-request';

@Injectable({
  providedIn: 'root',
})
export class EndorsementRequestsReceivedResource extends CrudResource<ReceivedEndorsementRequest> {
  public constructor(
    private apiResource: ApiHttpClient
  ) {
    super(apiResource);
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

  public getReceivedEndorsementRequests(partyId: number): Observable<ReceivedEndorsementRequest[]> {
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

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsement-requests/recieved`;
  }
}
