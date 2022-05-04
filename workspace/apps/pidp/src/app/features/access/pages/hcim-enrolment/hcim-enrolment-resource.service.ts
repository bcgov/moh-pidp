import { Injectable } from '@angular/core';

import { Observable, catchError, map, of } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { HcimEnrolment } from './hcim-enrolment.model';

export enum HcimEnrolmentStatusCode {
  ACCESS_GRANTED,
  ACCESS_FAILED,
}

export interface HcimEnrolmentResponse {
  statusCode: HcimEnrolmentStatusCode;
}

@Injectable({
  providedIn: 'root',
})
export class HcimEnrolmentResource {
  public constructor(
    protected apiResource: ApiHttpClient,
    private portalResource: PortalResource
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(
    partyId: number,
    hcimEnrolment: HcimEnrolment
  ): Observable<HcimEnrolmentResponse> {
    return this.apiResource
      .post<NoContent>('access-requests/hcim-enrolment', {
        partyId,
        ...hcimEnrolment,
      })
      .pipe(
        map(() => ({ statusCode: HcimEnrolmentStatusCode.ACCESS_GRANTED })),
        catchError(() =>
          of({ statusCode: HcimEnrolmentStatusCode.ACCESS_FAILED })
        )
      );
  }
}
