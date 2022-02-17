import { Injectable } from '@angular/core';

/**
 * @description
 * PartyService used to store Party specific information
 * requested when a user routes into an authenticated route
 * configuration for use within the application.
 *
 * WARNING: Depends on the use of the PartyResolver!
 */
@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private _partyId!: number;

  /**
   * @description
   * Single use setter for the Party identifier, which
   * should only be used by the PartyResolver.
   */
  public set partyId(partyId: number) {
    if (!this._partyId) {
      this._partyId = partyId;
    }
  }

  public get partyId(): number {
    return this._partyId;
  }
}
