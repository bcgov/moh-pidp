import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, switchMap, throwError } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { PartyCreate } from './party-create.model';

@Injectable({
  providedIn: 'root',
})
export class PartyResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private authorizedUserService: AuthorizedUserService
  ) {}

  /**
   * @description
   * Get a party ID based on the access token user ID, and
   * create a party if one does not already exist.
   */
  public firstOrCreate(): Observable<number> {
    return this.apiResource.post<number>('discovery', null).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status !== 404) {
          return throwError(() => error);
        }

        return this.authorizedUserService.user$.pipe(
          switchMap((user: User) => {
            return user
              ? this.createParty(user)
              : throwError(
                  () =>
                    new Error(
                      'Not authenticated or access token could not be parsed'
                    )
                );
          })
        );
      })
    );
  }

  /**
   * @description
   * Create a new party from information provided from the
   * access token.
   */
  private createParty(partyCreate: PartyCreate): Observable<number> {
    return this.apiResource.post<number>('parties', partyCreate);
  }
}
