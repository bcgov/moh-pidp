import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimWebEnrolmentRoutingModule } from './hcim-reenrolment-routing.module';
import { HcimReenrolmentPage } from './hcim-reenrolment.page';

@NgModule({
  declarations: [HcimReenrolmentPage],
  imports: [HcimWebEnrolmentRoutingModule, SharedModule],
})
export class HcimWebEnrolmentModule {}
