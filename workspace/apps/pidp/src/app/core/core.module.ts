import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { httpInterceptorProviders } from '@bcgov/shared/data-access';
import { rootRouteConfigProvider } from '@bcgov/shared/ui';
import { EnsureModuleLoadedOnceGuard } from '@bcgov/shared/utils';

const modules = [
  BrowserModule,
  HttpClientModule,
  BrowserAnimationsModule,
  // TODO only applied to allow for a few core services until moved
  ReactiveFormsModule,
  // TODO only applied to allow for a few core services until moved
  MatSnackBarModule,
];

@NgModule({
  imports: modules,
  providers: [rootRouteConfigProvider, httpInterceptorProviders],
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }
}
