import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { BcProviderApplicationRoutingModule } from './bc-provider-application-routing.module';
import { BcProviderApplicationPage } from './bc-provider-application.page';

@NgModule({
  declarations: [BcProviderApplicationPage],
  imports: [BcProviderApplicationRoutingModule, SharedModule],
})
export class BcProviderApplicationModule {}
