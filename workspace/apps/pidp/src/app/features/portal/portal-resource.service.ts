import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ProfileStatus } from './models/profile-status.model';

@Injectable({
  providedIn: 'root',
})
export class PortalResource {
  private apiResource = inject(ApiHttpClient);


  public getProfileStatus(partyId: number): Observable<ProfileStatus | null> {
    if (!partyId) {
      return of(null);
    }
    return this.apiResource
      .get<ProfileStatus>(`parties/${partyId}/profile-status`)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        }),
      );
  }
}
