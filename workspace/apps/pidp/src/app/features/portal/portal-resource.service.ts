import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ProfileStatus } from './models/profile-status.model';

@Injectable({
  providedIn: 'root',
})
export class PortalResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    return this.apiResource
      .post<ProfileStatus>(`parties/${partyId}/profile-status`, {})
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        })
      );
  }
}
