import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  AbstractResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from './api-http-client.service';

@Injectable({
  providedIn: 'root',
})
export class AccessRequestResource extends AbstractResource {
  public constructor(private apiResource: ApiHttpClient) {
    super('access-requests');
  }

  public saEforms(partyId: number): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.resourceBaseUri}/sa-eforms`, { partyId })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of();
          }

          return throwError(() => error);
        })
      );
  }
}
