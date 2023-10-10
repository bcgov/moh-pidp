import { Injectable } from '@angular/core';

import { Observable, map, of, switchMap, throwError } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { PartyCreate } from './party-create.model';

export enum Destination {
  NEW_USER = 1,
  NEW_BC_PROVIDER,
  DEMOGRAPHICS,
  USER_ACCESS_AGREEMENT,
  PORTAL,
}

export interface DiscoveryResult {
  partyId?: number;
  destination: Destination;
}

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
   * Get the party's ID based on the access token, as well as their destination / progress through the initial wizard.
   * Creates a party if one does not already exist.
   */
  public discover(): Observable<DiscoveryResult> {
    return this.apiResource.post<DiscoveryResult>('discovery', null).pipe(
      switchMap((result) => {
        return result.destination !== Destination.NEW_USER
          ? of(result)
          : this.authorizedUserService.user$.pipe(
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
  private createParty(partyCreate: PartyCreate): Observable<DiscoveryResult> {
    return this.apiResource.post<number>('parties', partyCreate).pipe(
      map((partyId) => ({
        partyId: partyId,
        destination: Destination.DEMOGRAPHICS,
      }))
    );
  }
}
