import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

export interface BcProviderCreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
}
@Injectable({
  providedIn: 'root',
})
export class BcProviderResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public createUser(
    request: BcProviderCreateUserRequest
  ): Observable<string | null> {
    return this.apiResource.post<string>(`bcprovider`, request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      })
    );
  }
}
