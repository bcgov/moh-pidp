import { Injectable } from '@angular/core';

import { Observable, map, of } from 'rxjs';

import { SortUtils } from '@bcgov/shared/utils';

import { LookupResource } from './lookup-resource.service';
import {
  CollegeLookup,
  Lookup,
  LookupConfig,
  ProvinceLookup,
} from './lookup.types';

export interface ILookupService extends LookupConfig {
  load(): Observable<LookupConfig | null>;
}

@Injectable({
  providedIn: 'root',
})
export class LookupService implements ILookupService {
  private lookupConfig: LookupConfig | null;

  public constructor(private lookupResource: LookupResource) {
    this.lookupConfig = null;
  }

  public get accessTypes(): Lookup[] {
    return this.copyAndSortByKey(this.lookupConfig?.accessTypes);
  }

  public get colleges(): CollegeLookup[] {
    return this.copyAndSortByKey<CollegeLookup>(this.lookupConfig?.colleges);
  }

  public get countries(): Lookup<string>[] {
    return this.copyAndSortByKey<Lookup<string>>(
      this.lookupConfig?.countries,
      'name',
    );
  }

  public get provinces(): ProvinceLookup[] {
    return this.copyAndSortByKey<ProvinceLookup>(
      this.lookupConfig?.provinces,
      'name',
    );
  }

  public get organizations(): Lookup[] {
    return this.copyAndSortByKey(this.lookupConfig?.organizations);
  }

  public get healthAuthorities(): Lookup[] {
    return this.copyAndSortByKey(this.lookupConfig?.healthAuthorities);
  }

  /**
   * @description
   * Load the runtime lookups, otherwise use a locally
   * cached version of the lookups.
   */
  public load(): Observable<LookupConfig | null> {
    return !this.lookupConfig
      ? this.lookupResource
          .getLookups()
          .pipe(
            map(
              (lookupConfig: LookupConfig | null) =>
                (this.lookupConfig = lookupConfig),
            ),
          )
      : of({ ...this.lookupConfig });
  }
  /**
   * Helper method so the calling code does not need to know about how this class implements the college lookup.
   * @param collegeCode
   * @returns
   */
  public getCollege(collegeCode: number): Lookup | null {
    const college = this.colleges.find((x) => x.code === collegeCode);
    return college ?? null;
  }

  /**
   * @description
   * Make a copy of the lookup so it won't be overwritten by
   * reference within the service, and then sort by key.
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private copyAndSortByKey<T extends { [key: string]: any } = Lookup>(
    lookup: T[] | undefined,
    sortBy: keyof T = 'code' as keyof T,
  ): T[] {
    return lookup?.length
      ? [...lookup].sort(SortUtils.sortByKey<T>(sortBy))
      : [];
  }
}
