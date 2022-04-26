import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { HcimEnrolmentRoutingModule } from './hcim-enrolment-routing.module';
import { HcimEnrolmentComponent } from './hcim-enrolment.component';

@NgModule({
  declarations: [HcimEnrolmentComponent],
  imports: [CommonModule, HcimEnrolmentRoutingModule],
})
export class HcimEnrolmentModule {}
