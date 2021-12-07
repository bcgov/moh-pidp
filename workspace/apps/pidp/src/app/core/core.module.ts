import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { httpInterceptorProviders } from '@bcgov/shared/data-access';
import { EnsureModuleLoadedOnceGuard } from '@bcgov/shared/utils';

import { rootRouteConfigProvider } from '../modules/root-routing/root-route.config';

const modules = [BrowserModule, BrowserAnimationsModule, HttpClientModule];

@NgModule({
  imports: modules,
  providers: [rootRouteConfigProvider, httpInterceptorProviders],
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }
}
