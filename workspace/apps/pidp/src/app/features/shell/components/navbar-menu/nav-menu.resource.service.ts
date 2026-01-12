import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { Credential } from './nav-menu.model';

@Injectable({
  providedIn: 'root',
})
export class NavMenuResource {
  public constructor(protected apiResource: ApiHttpClient) {}

  public getCredentials(partyId: number): Observable<Credential[] | null> {
    if (!partyId) {
      return of(null);
    }
    return this.apiResource
      .get<Credential[]>(this.getResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
