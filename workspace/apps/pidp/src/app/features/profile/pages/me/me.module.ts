import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { MeRoutingModule } from './me-routing.module';
import { MePage } from './me.page';

@NgModule({
  declarations: [MePage],
  imports: [MeRoutingModule, SharedModule],
})
export class MeModule {}
