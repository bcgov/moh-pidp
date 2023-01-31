import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';

import { Observable } from 'rxjs';

import { PartyService } from '@app/core/party/party.service';

import { BcProviderEditInitialStateModel } from './bc-provider-edit.component';
import { BcProviderEditResource } from './bc-provider-edit.resource';

@Injectable({ providedIn: 'root' })
export class BcProviderEditResolver
  implements Resolve<BcProviderEditInitialStateModel>
{
  public constructor(
    private bcProviderEditResource: BcProviderEditResource,
    private partyService: PartyService
  ) {}
  public resolve(): Observable<BcProviderEditInitialStateModel> {
    const partyId = this.partyService.partyId;
    return this.bcProviderEditResource.get(partyId);
  }
}
