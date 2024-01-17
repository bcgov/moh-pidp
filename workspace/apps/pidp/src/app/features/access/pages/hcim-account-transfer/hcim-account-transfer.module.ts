import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimAccountTransferRoutingModule } from './hcim-account-transfer-routing.module';
import { HcimAccountTransferPage } from './hcim-account-transfer.page';

@NgModule({
  imports: [
    HcimAccountTransferRoutingModule,
    SharedModule,
    HcimAccountTransferPage,
  ],
})
export class HcimAccountTransferModule {}
