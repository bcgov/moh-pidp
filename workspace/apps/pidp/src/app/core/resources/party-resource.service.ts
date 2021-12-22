import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { Party, PartyCreate } from '@bcgov/shared/data-access';

import { ApiResource } from './api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class PartyResource {
  public constructor(private apiResource: ApiResource) {}

  public getParties(): Observable<Party[] | null> {
    return this.apiResource.get<Party[]>('parties').pipe(
      this.apiResource.unwrapResultPipe(),
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    );
  }

  public createParty(party: PartyCreate): Observable<number | null> {
    return this.apiResource.post<number>('parties', party).pipe(
      this.apiResource.unwrapResultPipe(),
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    );
  }

  public getParty(partyId: number): Observable<Party | null> {
    return this.apiResource.get<Party>(`parties/${partyId}`).pipe(
      this.apiResource.unwrapResultPipe(),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      })
    );
  }
}
