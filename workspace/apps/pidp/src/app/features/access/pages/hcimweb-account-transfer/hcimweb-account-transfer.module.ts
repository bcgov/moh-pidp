import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimwebAccountTransferRoutingModule } from './hcimweb-account-transfer-routing.module';
import { HcimwebAccountTransferPage } from './hcimweb-account-transfer.page';

@NgModule({
  declarations: [HcimwebAccountTransferPage],
  imports: [HcimwebAccountTransferRoutingModule, SharedModule],
})
export class HcimwebAccountTransferModule {}
