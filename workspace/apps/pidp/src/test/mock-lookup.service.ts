import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { LookupResource } from '@app/modules/lookup/lookup-resource.service';
import { LookupConfig } from '@app/modules/lookup/lookup.model';
import {
  ILookupService,
  LookupService,
} from '@app/modules/lookup/lookup.service';

import { UtilsService } from '@core/services/utils.service';

import { MockLookup } from './mock-lookup';

@Injectable({
  providedIn: 'root',
})
export class MockLookupService extends LookupService implements ILookupService {
  public constructor(
    protected lookupResource: LookupResource,
    protected utilsService: UtilsService
  ) {
    super(lookupResource, utilsService);

    // Load the runtime configuration
    this.load().subscribe();
  }

  public load(): Observable<LookupConfig> {
    return new Observable<LookupConfig>((subscriber) => {
      const lookupConfig = MockLookup.get();

      subscriber.next((this.lookupConfig = lookupConfig));
      subscriber.complete();
    });
  }
}
