import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { httpInterceptorProviders } from '@bcgov/shared/data-access';
import { NgxProgressBarModule, RootRoutingModule } from '@bcgov/shared/ui';
import { EnsureModuleLoadedOnceGuard } from '@bcgov/shared/utils';

import { KeycloakModule } from '@app/modules/keycloak/keycloak.module';
import { LookupModule } from '@app/modules/lookup/lookup.module';
import { PermissionsModule } from '@app/modules/permissions/permissions.module';

const modules = [
  BrowserModule,
  BrowserAnimationsModule,
  HttpClientModule,
  // TODO required at root to connect with HttpClientModule
  NgxProgressBarModule,
  // TODO only applied to allow for a few core services until moved
  ReactiveFormsModule,
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
