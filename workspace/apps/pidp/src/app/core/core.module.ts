import { NgModule, Optional, SkipSelf } from '@angular/core';
import { maintenanceInterceptorProvider } from '../libs/data-access/interceptors/maintenance.interceptor';

import { unauthorizedInterceptorProvider } from '../libs/data-access/interceptors/unauthorized.interceptor';
import { EnsureModuleLoadedOnceGuard } from '../libs/utils/classes/ensure-module-loaded-once-guard.class';

@NgModule({
  declarations: [],
  imports: [],
  providers: [unauthorizedInterceptorProvider, maintenanceInterceptorProvider],
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    super(parentModule);
  }
}
