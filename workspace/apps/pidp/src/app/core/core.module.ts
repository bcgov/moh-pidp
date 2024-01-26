import { NgModule, Optional, SkipSelf } from '@angular/core';

import { EnsureModuleLoadedOnceGuard } from '@bcgov/shared/utils';

import { KeycloakModule } from '@app/modules/keycloak/keycloak.module';
import { LookupModule } from '@app/modules/lookup/lookup.module';
import { PermissionsModule } from '@app/modules/permissions/permissions.module';

const modules = [
  LookupModule.forRoot(),
  KeycloakModule,
  PermissionsModule.forRoot(),
];

@NgModule({
  imports: modules,
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }
}
