import { Injectable } from '@angular/core';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { OrganizationDetails } from './organization-details.model';

@Injectable({
  providedIn: 'root',
})
export class OrganizationDetailsResource extends CrudResource<OrganizationDetails> {
  public constructor(protected apiResource: ApiHttpClient) {
    super(apiResource);
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/organization-details`;
  }
}
