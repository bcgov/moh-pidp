import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, Observable } from 'rxjs';

import { Party } from '@bcgov/shared/data-access';

import { ApiResource } from './api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class PartyResource {
  public constructor(private apiResource: ApiResource) {}

  // TODO reduce typing for results by wrapping HttpResponse
  public getParties(): Observable<Party[] | null> {
    return this.apiResource.get<Party[]>('parties').pipe(
      this.apiResource.unwrapResultPipe(),
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    );
  }

  public createParty(party: Party): Observable<Party | null> {
    return this.apiResource.post<Party>('parties', party).pipe(
      this.apiResource.unwrapResultPipe(),
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    );
  }
}
