import { APP_INITIALIZER, Provider } from '@angular/core';

import { Observable } from 'rxjs';

import { LookupService } from './lookup.service';
import { LookupConfig } from './lookup.types';

function configFactory(
  lookupService: LookupService,
): () => Observable<LookupConfig | null> {
  // Ensure configuration is populated before the application
  // is fully initialized to prevent race conditions
  return (): Observable<LookupConfig | null> => lookupService.load();
}

export function provideLookup(): Provider {
  return {
    provide: APP_INITIALIZER,
    useFactory: configFactory,
    multi: true,
    deps: [LookupService],
  };
}
