import { Injectable } from '@angular/core';

import { Observable, catchError, map, of } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { LookupConfig } from './lookup.types';

@Injectable({
  providedIn: 'root',
})
export class LookupResource {
  public constructor(private apiResource: ApiHttpClient) {}

  /**
   * @description
   * Get the lookups for bootstrapping the application.
   */
  public getLookups(): Observable<LookupConfig | null> {
    return this.apiResource.get<LookupConfig>('lookups').pipe(
      map((lookupConfig: LookupConfig) => lookupConfig),
      // Catch and release to allow the application to render
      // views regardless of the presence of the lookups
      catchError((_) => of(null))
    );
  }
}
