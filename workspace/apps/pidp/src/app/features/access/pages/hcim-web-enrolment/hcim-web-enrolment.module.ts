import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimWebEnrolmentRoutingModule } from './hcim-web-enrolment-routing.module';
import { HcimWebEnrolmentPage } from './hcim-web-enrolment.page';

@NgModule({
  declarations: [HcimWebEnrolmentPage],
  imports: [HcimWebEnrolmentRoutingModule, SharedModule],
})
export class HcimWebEnrolmentModule {}
