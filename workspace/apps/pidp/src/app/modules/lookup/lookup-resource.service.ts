import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { catchError, Observable, of } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { ApiResource } from '@core/resources/api-resource.service';

import { LookupConfig } from './lookup.model';

@Injectable({
  providedIn: 'root',
})
export class LookupResource {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private http: HttpClient,
    private apiResource: ApiResource
  ) {}

  /**
   * @description
   * Get the lookups for bootstrapping the application.
   */
  public getLookups(): Observable<LookupConfig | null> {
    return this.http
      .get<LookupConfig>(`${this.config.apiEndpoint}/lookups`)
      .pipe(
        // Catch and release to allow the application to render
        // views regardless of the presence of the lookups
        catchError((_) => of(null))
      );
  }
}
