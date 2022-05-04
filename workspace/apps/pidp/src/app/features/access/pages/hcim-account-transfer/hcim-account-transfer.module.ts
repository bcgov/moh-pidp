import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimAccountTransferRoutingModule } from './hcim-account-transfer-routing.module';
import { HcimAccountTransferPage } from './hcim-account-transfer.page';

@NgModule({
  declarations: [HcimAccountTransferPage],
  imports: [HcimAccountTransferRoutingModule, SharedModule],
})
export class HcimAccountTransferModule {}
