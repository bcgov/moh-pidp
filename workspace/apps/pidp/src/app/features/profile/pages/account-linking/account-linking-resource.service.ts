import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';

//TODO move to new file once we have shape for LinkedAccount
export interface LinkedAccount {
  id: number;
}

@Injectable({
  providedIn: 'root',
})
export class AccountLinkingResource {
  public constructor(
    protected apiResource: ApiHttpClient,
    private portalResource: PortalResource,
  ) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.portalResource.getProfileStatus(partyId);
  }

  public linkAccounts(id: number): NoContent {
    return this.apiResource
      .post<NoContent>(this.getResourcePath(id), null)
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  public getLinkedAccounts(
    partyId: number,
  ): Observable<LinkedAccount[] | undefined> {
    return this.apiResource
      .get<LinkedAccount[]>(`${this.getResourcePath(partyId)}/linked-accounts`)
      .pipe(
        NoContentResponse,
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/profile/account-linking`;
  }
}
