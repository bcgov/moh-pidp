import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { HcimReenrolmentModel } from './hcim-reenrolment.model';

@Injectable()
export class HcimReenrolmentResource extends CrudResource<HcimReenrolmentModel> {
  public constructor(protected apiResource: ApiHttpClient) {
    super(apiResource);
  }

  // TODO toast success so messaged during routing
  // TODO what responses are expected?
  // TODO are we tracking attempts?

  // public hcimReEnrolment(
  //   partyId: number,
  //   ldapUsername: string,
  //   ldapPassword: string
  // ): NoContent {
  //   return this.apiResource
  //     .post<NoContent>(`${this.resourceBaseUri}/hcim-re-enrolment`, {
  //       partyId,
  //       ldapUsername,
  //       ldapPassword,
  //     })
  //     .pipe(
  //       NoContentResponse,
  //       catchError((error: HttpErrorResponse) => {
  //         if (error.status === HttpStatusCode.BadRequest) {
  //           return of(void 0);
  //         }

  //         return throwError(() => error);
  //       })
  //     );
  // }

  public requestAccess(
    partyId: number,
    ldapCredentials: { ldapUsername: string; ldapPassword: string }
  ): NoContent {
    return this.apiResource
      .post<NoContent>('access-requests/hcim-reenrolment', {
        partyId,
        ...ldapCredentials,
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

  protected getResourcePath(partyId: number): string {
    // TODO need resource endpoint
    return `parties/${partyId}/hcim-reenrolment`;
  }
}
