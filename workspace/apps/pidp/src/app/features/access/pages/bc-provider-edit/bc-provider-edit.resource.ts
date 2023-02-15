import { Injectable } from '@angular/core';

import { Observable, map } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { BcProviderEditInitialStateModel } from './bc-provider-edit.component';

export interface BcProviderChangePasswordRequest {
  partyId: number;
  newPassword: string;
}

@Injectable({ providedIn: 'root' })
export class BcProviderEditResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public get(partyId: number): Observable<BcProviderEditInitialStateModel> {
    const url = `parties/${partyId}/credentials/bc-provider`;
    return this.apiResource.get<BcProviderEditInitialStateModel>(url);
  }
  public changePassword(
    data: BcProviderChangePasswordRequest
  ): Observable<boolean> {
    const url = `parties/${data.partyId}/credentials/bc-provider/password`;
    return this.apiResource.post<NoContent>(url, data).pipe(map((_) => true));
  }
}
