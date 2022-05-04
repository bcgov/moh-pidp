import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimEnrolmentRoutingModule } from './hcim-enrolment-routing.module';
import { HcimEnrolmentComponent } from './hcim-enrolment.component';

@NgModule({
  declarations: [HcimEnrolmentComponent],
  imports: [HcimEnrolmentRoutingModule, SharedModule],
})
export class HcimEnrolmentModule {}
