import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { UciRoutingModule } from './uci-routing.module';
import { UciPage } from './uci.page';

@NgModule({
  declarations: [UciPage],
  imports: [SharedModule, UciRoutingModule, AccessModule],
})
export class UciModule {}
