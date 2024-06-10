import { Pipe, PipeTransform } from '@angular/core';

import { LookupService } from './lookup.service';
import { Lookup, LookupConfig } from './lookup.types';

@Pipe({
  name: 'lookupCode',
  standalone: true,
})
export class LookupCodePipe implements PipeTransform {
  public constructor(private lookupService: LookupService) {}

  public transform<T extends string | number>(
    lookupCode: T | null | undefined,
    lookupKey: string,
    key = 'name',
  ): unknown | null {
    return lookupCode && lookupKey && key
      ? this.lookupValue<T>(lookupCode, lookupKey, key)
      : null;
  }

  private lookupValue<T extends number | string>(
    lookupCode: T,
    lookupKey: string,
    key: string,
  ): unknown | null {
    const lookupConfig = this.lookupService[lookupKey as keyof LookupConfig];
    const lookup = (lookupConfig as Lookup[])?.find(
      (l: Lookup) => l.code === lookupCode,
    );

    return lookup && Object.prototype.hasOwnProperty.call(lookup, key)
      ? lookup[key as keyof Lookup]
      : null;
  }
}
