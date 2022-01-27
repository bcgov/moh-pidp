import { Injectable } from '@angular/core';

import { Observable, catchError, map, of } from 'rxjs';

import { ApiResource } from '@core/resources/api-resource.service';

import { LookupConfig } from './lookup.model';

@Injectable({
  providedIn: 'root',
})
export class LookupResource {
  public constructor(private apiResource: ApiResource) {}

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
