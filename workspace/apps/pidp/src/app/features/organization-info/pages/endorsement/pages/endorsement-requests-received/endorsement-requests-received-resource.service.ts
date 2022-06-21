import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PersonalInformation } from '@app/features/profile/pages/personal-information/personal-information.model';

@Injectable({
  providedIn: 'root',
})
export class EndorsementRequestsReceivedResource extends CrudResource<PersonalInformation> {
  public constructor(
    private apiResource: ApiHttpClient
  ) {
    super(apiResource);
  }

  public recieveEndorsementRequest(partyId: number, token: string): NoContent {
    return this.apiResource
      .post<NoContent>(`parties/${partyId}/endorsement-requests/recieved`, {
        token,
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
    return `parties/${partyId}/endorsement-requests/recieved`;
  }
}
