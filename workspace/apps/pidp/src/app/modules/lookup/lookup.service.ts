import { Injectable } from '@angular/core';

import { Observable, map, of } from 'rxjs';

import { UtilsService } from '@core/services/utils.service';

import { LookupResource } from './lookup-resource.service';
import {
  CollegeLookup,
  Lookup,
  LookupConfig,
  ProvinceLookup,
} from './lookup.model';

export interface ILookupService extends LookupConfig {
  load(): Observable<LookupConfig | null>;
}

@Injectable({
  providedIn: 'root',
})
export class LookupService implements ILookupService {
  protected lookupConfig: LookupConfig | null;

  public constructor(
    protected lookupResource: LookupResource,
    protected utilsService: UtilsService
  ) {
    this.lookupConfig = null;
  }

  public get colleges(): CollegeLookup[] {
    const colleges = this.lookupConfig?.colleges ?? [];
    return [...colleges].sort(
      this.utilsService.sortByKey<CollegeLookup>('code')
    );
  }

  public get countries(): Lookup<string>[] {
    const countries = this.lookupConfig?.countries ?? [];
    return countries.length
      ? [...countries].sort(this.utilsService.sortByKey<Lookup<string>>('name'))
      : [];
  }

  public get provinces(): ProvinceLookup[] {
    const provinces = this.lookupConfig?.provinces ?? [];
    return provinces.length
      ? [...provinces].sort(this.utilsService.sortByKey<ProvinceLookup>('name'))
      : [];
  }

  /**
   * @description
   * Load the runtime lookups.
   */
  public load(): Observable<LookupConfig | null> {
    if (!this.lookupConfig) {
      return this.lookupResource
        .getLookups()
        .pipe(
          map(
            (lookupConfig: LookupConfig | null) =>
              (this.lookupConfig = lookupConfig)
          )
        );
    }

    return of({ ...this.lookupConfig });
  }
}
