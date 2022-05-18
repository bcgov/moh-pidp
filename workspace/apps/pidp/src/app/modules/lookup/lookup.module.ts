import { APP_INITIALIZER, ModuleWithProviders, NgModule } from '@angular/core';

import { Observable } from 'rxjs';

import { LookupCodePipe } from './lookup-code.pipe';
import { LookupService } from './lookup.service';
import { LookupConfig } from './lookup.types';

function configFactory(
  lookupService: LookupService
): () => Observable<LookupConfig | null> {
  // Ensure configuration is populated before the application
  // is fully initialized to prevent race conditions
  return (): Observable<LookupConfig | null> => lookupService.load();
}

@NgModule({
  declarations: [LookupCodePipe],
  exports: [LookupCodePipe],
})
export class LookupModule {
  public static forRoot(): ModuleWithProviders<LookupModule> {
    return {
      ngModule: LookupModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          useFactory: configFactory,
          multi: true,
          deps: [LookupService],
        },
        LookupCodePipe,
      ],
    };
  }

  public static forChild(): ModuleWithProviders<LookupModule> {
    return {
      ngModule: LookupModule,
      providers: [LookupCodePipe],
    };
  }
}
