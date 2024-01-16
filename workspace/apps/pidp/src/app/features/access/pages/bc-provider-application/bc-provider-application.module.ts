import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { BcProviderApplicationRoutingModule } from './bc-provider-application-routing.module';
import { BcProviderApplicationPage } from './bc-provider-application.page';

@NgModule({
    imports: [BcProviderApplicationRoutingModule, SharedModule, BcProviderApplicationPage],
})
export class BcProviderApplicationModule {}
