import { Injectable } from '@angular/core';

import { Observable, map } from 'rxjs';

import { NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

export interface BcProviderApplicationRequest {
  partyId: number;
  identityProvider: IdentityProvider;
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class BcProviderApplicationResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public createBcProviderAccount(
    data: BcProviderApplicationRequest
  ): Observable<string> {
    return this.apiResource
      .post<NoContent>('credentials', data)
      .pipe(map((_) => ''));
  }
}
