import { NgModule, Optional, SkipSelf } from '@angular/core';

import { EnsureModuleLoadedOnceGuard } from '../libs/utils/classes/ensure-module-loaded-once-guard.class';
import { unauthorizedInterceptorProvider } from '../libs/data-access/interceptors/unauthorized.interceptor';
import { maintenanceInterceptorProvider } from '../libs/data-access/interceptors/maintenance.interceptor';

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
