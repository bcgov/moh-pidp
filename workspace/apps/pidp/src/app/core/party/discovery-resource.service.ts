import { Injectable } from '@angular/core';

import { Observable, map, of, switchMap, throwError } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { PartyCreate } from './party-create.model';

export interface DiscoveryResult {
  partyId?: number;
  newBCProvider: boolean;
  status?: StatusCode;
}

export enum Destination {
  DEMOGRAPHICS = 1,
  USER_ACCESS_AGREEMENT,
  LICENCE_DECLARATION,
  PORTAL,
}
export enum StatusCode {
  Success = 1,
  NewUser,
  NewBCProviderError,
  AlreadyLinkedError,
  CredentialExistsError,
  ExpiredCredentialLinkTicketError,
}

@Injectable({
  providedIn: 'root',
})
export class DiscoveryResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private authorizedUserService: AuthorizedUserService,
  ) { }

  /**
   * @description
   * Get the party's ID based on the access token user ID,
   * creates a party if one does not already exist,
   * or discovers that the User is a new BC Provider (and so cannot create a Party).
   */
  public discover(): Observable<DiscoveryResult> {
    return this.apiResource.post<DiscoveryResult>('discovery', null).pipe(
      switchMap((result) => {
        return result.partyId || result.newBCProvider
          ? of(result)
          : this.authorizedUserService.user$.pipe(
            switchMap((user: User) => {
              return user
                ? this.createParty(user)
                : throwError(
                  () =>
                    new Error(
                      'Not authenticated or access token could not be parsed',
                    ),
                );
            }),
          );
      }),
    );
  }

  public getDestination(partyId: number): Observable<Destination> {
    return this.apiResource
      .get<{ destination: Destination }>(`discovery/${partyId}/destination`)
      .pipe(map((result) => result.destination));
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
        newBCProvider: false,
      })),
    );
  }
}
