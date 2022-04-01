import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { ProfileStatus } from '@app/features/portal/sections/models/profile-status.model';

import { HcimReenrolment } from './hcim-reenrolment.model';

@Injectable({
  providedIn: 'root',
})
export class HcimReenrolmentResource {
  public constructor(
    protected apiResource: ApiHttpClient,
    private portalResource: PortalResource
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(
    partyId: number,
    ldapCredentials: HcimReenrolment
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
}
