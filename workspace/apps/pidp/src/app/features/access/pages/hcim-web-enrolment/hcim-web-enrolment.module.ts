import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimWebEnrolmentRoutingModule } from './hcim-web-enrolment-routing.module';
import { HcimWebEnrolmentComponent } from './hcim-web-enrolment.component';

@NgModule({
  declarations: [HcimWebEnrolmentComponent],
  imports: [HcimWebEnrolmentRoutingModule, SharedModule],
})
export class HcimWebEnrolmentModule {}
