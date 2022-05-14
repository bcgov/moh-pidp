import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PersonalInformation } from '@app/features/profile/pages/personal-information/personal-information.model';

import { Endorsement } from './endorsement.model';

@Injectable({
  providedIn: 'root',
})
export class EndorsementResource extends CrudResource<PersonalInformation> {
  public constructor(
    private apiResource: ApiHttpClient,
    private portalResource: PortalResource
  ) {
    super(apiResource);
  }

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(partyId: number, endorsement: Endorsement): NoContent {
    return this.apiResource
      .post<NoContent>(this.getResourcePath(), {
        partyId,
        ...endorsement,
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

  protected getResourcePath(): string {
    return `access-requests/endorsement`;
  }
}
