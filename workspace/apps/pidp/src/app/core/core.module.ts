import { NgModule, Optional, SkipSelf } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { httpInterceptorProviders } from '@bcgov/shared/data-access';
import { NgxProgressBarModule, RootRoutingModule } from '@bcgov/shared/ui';
import { EnsureModuleLoadedOnceGuard } from '@bcgov/shared/utils';

import { KeycloakModule } from '@app/modules/keycloak/keycloak.module';
import { LookupModule } from '@app/modules/lookup/lookup.module';
import { PermissionsModule } from '@app/modules/permissions/permissions.module';

const modules = [
  // TODO required at root to connect with HttpClientModule
  NgxProgressBarModule,
  // TODO only applied to allow for a few core services until moved
  ReactiveFormsModule,
  // TODO only applied to allow for a few core services until moved
  MatDialogModule,
  // TODO only applied to allow for a few core services until moved
  MatSnackBarModule,
  LookupModule.forRoot(),
  KeycloakModule,
  PermissionsModule.forRoot(),
  RootRoutingModule,
];

@NgModule({
  imports: modules,
  providers: [httpInterceptorProviders],
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }
}
