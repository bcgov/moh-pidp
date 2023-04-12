import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  NoContentResponse,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { PrivacyOfficer } from './ms-teams-clinic-member.model';

@Injectable({
  providedIn: 'root',
})
export class MsTeamsClinicMemberResource extends CrudResource<PrivacyOfficer> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private portalResource: PortalResource,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public requestAccess(partyId: number, clinicId: number): NoContent {
    return this.apiResource
      .post<NoContent>(
        `parties/${partyId}/access-requests/ms-teams-clinic-member`,
        { clinicId }
      )
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        })
      );
  }

  public get(partyId: number): Observable<PrivacyOfficer | null> {
    return super.get(partyId).pipe(
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'MS Teams clinic information could not be retrieved'
        );
        throw error;
      })
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/`;
  }
}
