import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, of, throwError } from 'rxjs';

import { AbstractResource } from '@bcgov/shared/data-access';

import { ApiResource } from './api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class AccessRequestResource extends AbstractResource {
  public constructor(private apiResource: ApiResource) {
    super('access-requests');
  }

  public saEforms(partyId: number): Observable<boolean> {
    return this.apiResource
      .post<boolean>(`${this.resourceBaseUri}/sa-eforms`, { partyId })
      .pipe(
        map(() => {
          const accessAlreadyExists = false;
          return accessAlreadyExists;
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            const accessAlreadyExists = true;
            return of(accessAlreadyExists);
          }

          return throwError(() => error);
        })
      );
  }
}
