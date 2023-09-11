import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { BcProviderEditRoutingModule } from './bc-provider-edit-routing.module';
import { BcProviderEditPage } from './bc-provider-edit.page';

@NgModule({
  declarations: [BcProviderEditPage],
  imports: [BcProviderEditRoutingModule, SharedModule],
})
export class BcProviderEditModule {}
