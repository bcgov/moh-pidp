import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, exhaustMap, map, of, throwError } from 'rxjs';

import { Party } from '@bcgov/shared/data-access';

import { BcscUser } from '@app/features/auth/models/bcsc-user.model';

// TODO specific to app but should be with everything related to a party
import { PartyCreate } from '../models/party-create.model';
import { AuthorizedUserService } from '../services/authorized-user.service';
import { ApiHttpClient } from './api-http-client.service';
import { ResourceUtilsService } from './resource-utils.service';

@Injectable({
  providedIn: 'root',
})
export class PartyResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private resourceUtilsService: ResourceUtilsService,
    private authorizedUserService: AuthorizedUserService
  ) {}

  public getParty(partyId: number): Observable<Party | null> {
    return this.apiResource.get<Party>(`parties/${partyId}`).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      })
    );
  }

  /**
   * @description
   * Get a party ID based on the access token user ID, and
   * create a party if one does not already exist.
   */
  public firstOrCreate(): Observable<number | null> {
    return this.authorizedUserService.user$.pipe(
      exhaustMap((user: BcscUser | null) =>
        user
          ? this.getParties(user.userId).pipe(
              map((partyId: number | null) => partyId ?? user)
            )
          : throwError(
              () =>
                new Error(
                  'Not authenticated or access token could not be parsed'
                )
            )
      ),
      exhaustMap((partyIdOrBcscUser: number | BcscUser | null) =>
        typeof partyIdOrBcscUser === 'number' || !partyIdOrBcscUser
          ? of(partyIdOrBcscUser)
          : this.createParty(partyIdOrBcscUser)
      )
    );
  }

  /**
   * @description
   * Discovery endpoint for checking the existence of a Party
   * based on a UserId, which provides a PartyId in response.
   */
  private getParties(userId: string): Observable<number | null> {
    const params = this.resourceUtilsService.makeHttpParams({ userId });
    return this.apiResource.get<{ id: number }[]>('parties', { params }).pipe(
      map((parties: { id: number }[]) =>
        parties?.length ? parties.shift()?.id ?? null : null
      ),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          return of(null);
        }

        return throwError(
          () =>
            new Error(
              `Error occurred attempting to retrieve Party with user ID ${userId}`
            )
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
