import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, of } from 'rxjs';

import { NoContent, ResourceUtilsService } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { LookupConfig } from './lookup.types';

@Injectable({
  providedIn: 'root',
})
export class LookupResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private resourceUtilsService: ResourceUtilsService,
  ) {}

  /**
   * @description
   * Get the lookups for bootstrapping the application.
   */
  public getLookups(): Observable<LookupConfig | null> {
    return this.apiResource.get<LookupConfig>('lookups').pipe(
      map((lookupConfig: LookupConfig) => lookupConfig),
      // Catch and release to allow the application to render
      // views regardless of the presence of the lookups
      catchError((_) => of(null)),
    );
  }

  public hasCommonEmailDomain(email: string): Observable<boolean> {
    const params = this.resourceUtilsService.makeHttpParams({ email });
    return this.apiResource
      .head<NoContent>('lookups/common-email-domains', { params })
      .pipe(
        map(() => true),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            return of(false);
          }

          throw error;
        }),
      );
  }
}
