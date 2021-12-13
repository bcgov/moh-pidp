import { Pipe, PipeTransform } from '@angular/core';

import { Lookup, LookupConfig } from './lookup.model';
import { LookupService } from './lookup.service';

@Pipe({
  name: 'lookupCode',
})
export class LookupCodePipe implements PipeTransform {
  public constructor(private lookupService: LookupService) {}

  public transform<T extends string | number>(
    lookupCode: T | null | undefined,
    lookupKey: string,
    key: string = 'name'
  ): string | null {
    if (!lookupCode || !lookupKey) {
      return null;
    }

    return lookupCode ? this.lookupValue<T>(lookupCode, lookupKey, key) : '';
  }

  private lookupValue<T extends string | number>(
    lookupCode: T,
    lookupKey: string,
    key: string
  ): string | null {
    const lookupConfig = this.lookupService[lookupKey as keyof LookupConfig];

    if (!lookupConfig) {
      throw Error('Lookup key does not exist');
    }

    const lookup = lookupConfig.find((l) => l.code === lookupCode);

    return lookup && Object.prototype.hasOwnProperty.call(lookup, key)
      ? lookup[key as keyof Lookup]
      : null;
  }
}
