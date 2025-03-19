import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

@Injectable({
  providedIn: 'root',
})
export class B2bInvitationResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public inviteGuestAccount(partyId: number, upn: string): NoContent {
    return this.apiResource
      .post<string>(`${this.getResourcePath(partyId)}/bc-provider/invite`, {
        upn,
      })
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  private getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
