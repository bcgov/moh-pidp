import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  AbstractResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ToastService } from '../services/toast.service';
import { ApiHttpClient } from './api-http-client.service';

@Injectable({
  providedIn: 'root',
})
export class AccessRequestResource extends AbstractResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super('access-requests');
  }

  // TODO try and move this into associated module using Resolver
  public saEforms(partyId: number): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.resourceBaseUri}/sa-eforms`, { partyId })
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

  public hcimReEnrolment(
    partyId: number,
    ldapUsername: string,
    ldapPassword: string
  ): NoContent {
    return this.apiResource
      .post<NoContent>(`${this.resourceBaseUri}/hcim-re-enrolment`, {
        partyId,
        ldapUsername,
        ldapPassword,
      })
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
}
