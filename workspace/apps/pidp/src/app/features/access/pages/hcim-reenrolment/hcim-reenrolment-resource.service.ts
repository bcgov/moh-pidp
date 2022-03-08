import { Injectable } from '@angular/core';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { HcimReenrolmentModel } from './hcim-reenrolment.model';

@Injectable()
export class HcimReenrolmentResource extends CrudResource<HcimReenrolmentModel> {
  public constructor(protected apiResource: ApiHttpClient) {
    super(apiResource);
  }

  // TODO toast success so messaged during routing
  // TODO what responses are expected?
  // TODO are we tracking attempts?

  protected getResourcePath(partyId: number): string {
    // TODO need resource endpoint
    return `parties/${partyId}/lorem-ipsum`;
  }
}
