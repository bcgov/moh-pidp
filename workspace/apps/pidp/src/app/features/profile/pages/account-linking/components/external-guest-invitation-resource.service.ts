import { Injectable } from '@angular/core';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';


@Injectable({
  providedIn: 'root',
})
export class ExternalGuestInvitationResource {
  public constructor(
    protected apiResource: ApiHttpClient
  ) {}


  protected getResourcePath(partyId: number): string {
    return `place_holder/${partyId}/place_holder`;
  }
}
