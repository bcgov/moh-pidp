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
  ): unknown | null {
    if (!lookupCode || !lookupKey) {
      return null;
    }

    return lookupCode ? this.lookupValue<T>(lookupCode, lookupKey, key) : '';
  }

  private lookupValue<T extends number | string>(
    lookupCode: T,
    lookupKey: string,
    key: string
  ): unknown | null {
    const lookupConfig = this.lookupService[lookupKey as keyof LookupConfig];
    const lookup = (lookupConfig as Lookup<T>[])?.find(
      (l: Lookup<T>) => l.code === lookupCode
    );

    return lookup && Object.prototype.hasOwnProperty.call(lookup, key)
      ? lookup[key as keyof Lookup<T>]
      : null;
  }
}
