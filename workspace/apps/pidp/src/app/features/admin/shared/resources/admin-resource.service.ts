import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

export interface PartyList {
  id: number;
  providerName?: string;
  providerCollegeCode?: string;
  saEformsAccessRequest?: boolean;
}

// TODO temporary resource until more of the admin is known
@Injectable({
  providedIn: 'root',
})
export class AdminResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public getParties(): Observable<PartyList[]> {
    return this.apiResource.get<PartyList[]>('/admin/parties').pipe(
      catchError((_: HttpErrorResponse) => {
        // TODO add logging and toast messaging
        return of([]);
      })
    );
  }
}
