import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { ReceivedEndorsementRequest } from './models/received-endorsement-request.model';

@Injectable({
  providedIn: 'root',
})
export class EndorsementsResource extends CrudResource<ReceivedEndorsementRequest> {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource
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

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/endorsements`;
  }
}
