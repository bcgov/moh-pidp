import { ModuleWithProviders, NgModule } from '@angular/core';

import { FeatureFlagDirective } from './feature-flag.directive';
import { FeatureFlagService } from './feature-flag.service';

@NgModule({
  declarations: [FeatureFlagDirective],
  exports: [FeatureFlagDirective],
})
export class FeatureFlagModule {
  public static forRoot(): ModuleWithProviders<FeatureFlagModule> {
    return { ngModule: FeatureFlagModule, providers: [FeatureFlagService] };
  }

  public static forChild(): ModuleWithProviders<FeatureFlagModule> {
    return {
      ngModule: FeatureFlagModule,
      providers: [],
    };
  }
}
