import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  AbstractResource,
  NoContent,
  NoContentOrError,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiResource } from './api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class AccessRequestResource extends AbstractResource {
  public constructor(private apiResource: ApiResource) {
    super('access-requests');
  }

  public saEforms(partyId: number): NoContentOrError<null> {
    return this.apiResource
      .post<NoContent>(`${this.resourceBaseUri}/sa-eforms`, { partyId })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.UnprocessableEntity) {
            return of(null);
          }

          return throwError(() => error);
        })
      );
  }
}
