import { Injectable } from '@angular/core';

import { Observable, map } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { PartyCreate } from './party-create.model';

export interface DiscoveryResult {
  partyId?: number;
  status: DiscoveryStatus;
}

export enum Destination {
  DEMOGRAPHICS = 1,
  USER_ACCESS_AGREEMENT,
  LICENCE_DECLARATION,
  PORTAL,
}
export enum DiscoveryStatus {
  Success = 1,
  NewUser,
  NewBCProviderError,
  AccountLinkInProgress,
  AlreadyLinkedError,
  CredentialExistsError,
  ExpiredCredentialLinkTicketError,
  AccountLinkingError,
}

@Injectable({
  providedIn: 'root',
})
export class DiscoveryResource {
  public constructor(private apiResource: ApiHttpClient) {}

  /**
   * @description
   * Get the party's ID based on the access token user ID,
   * creates a party if one does not already exist,
   * or discovers that the User is a new BCProvider (and so cannot create a Party).
   */
  public discover(): Observable<DiscoveryResult> {
    return this.apiResource.post<DiscoveryResult>('discovery', null);
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
  public createParty(partyCreate: PartyCreate): Observable<DiscoveryResult> {
    return this.apiResource.post<number>('parties', partyCreate).pipe(
      map((partyId) => ({
        partyId: partyId,
        status: DiscoveryStatus.Success,
      })),
    );
  }
}
