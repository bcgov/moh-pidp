import { ModuleWithProviders, NgModule } from '@angular/core';

import { PermissionsDirective } from './permissions.directive';
import { PermissionsService } from './permissions.service';

@NgModule({
  declarations: [PermissionsDirective],
  exports: [PermissionsDirective],
})
export class PermissionsModule {
  public static forRoot(): ModuleWithProviders<PermissionsModule> {
    return { ngModule: PermissionsModule, providers: [PermissionsService] };
  }

  public static forChild(): ModuleWithProviders<PermissionsModule> {
    return {
      ngModule: PermissionsModule,
      providers: [],
    };
  }
}
