import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, exhaustMap, map, of, throwError } from 'rxjs';

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
  public firstOrCreate(): Observable<number | null> {
    return this.authorizedUserService.user$.pipe(
      exhaustMap((user: User) =>
        user
          ? this.getCredentials().pipe(
              map((partyId: number | null) => partyId ?? user)
            )
          : throwError(
              () =>
                new Error(
                  'Not authenticated or access token could not be parsed'
                )
            )
      ),
      exhaustMap((partyIdOrUser: number | User | null) =>
        typeof partyIdOrUser === 'number' || !partyIdOrUser
          ? of(partyIdOrUser)
          : this.createParty(partyIdOrUser)
      )
    );
  }

  /**
   * @description
   * Discovery endpoint for checking the existence of a Party
   * based on a UserId, which provides a PartyId in response.
   */
  private getCredentials(): Observable<number | null> {
    return this.apiResource.get<{ partyId: number }[]>('credentials').pipe(
      map((parties: { partyId: number }[]) =>
        parties?.length ? parties.shift()?.partyId ?? null : null
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return of(null);
        }

        return throwError(
          () =>
            new Error(`Error occurred attempting to retrieve Party Credentials`)
        );
      })
    );
  }

  /**
   * @description
   * Create a new party from information provided from the
   * access token.
   */
  private createParty(partyCreate: PartyCreate): Observable<number | null> {
    return this.apiResource.post<number>('parties', partyCreate).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return of(null);
        }

        return throwError(() => new Error('Party could not be created'));
      })
    );
  }
}
