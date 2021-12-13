import { APP_INITIALIZER, NgModule } from '@angular/core';

import { lastValueFrom } from 'rxjs';

import { LookupCodePipe } from './lookup-code.pipe';
import { LookupConfig } from './lookup.model';
import { LookupService } from './lookup.service';

function configFactory(
  lookupService: LookupService
): () => Promise<LookupConfig | null> {
  // Ensure configuration is populated before the application
  // is fully initialized to prevent race conditions
  return (): Promise<LookupConfig | null> =>
    lastValueFrom(lookupService.load());
}

@NgModule({
  declarations: [LookupCodePipe],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: configFactory,
      multi: true,
      deps: [LookupService],
    },
    LookupCodePipe,
  ],
  exports: [LookupCodePipe],
})
export class LookupModule {}
