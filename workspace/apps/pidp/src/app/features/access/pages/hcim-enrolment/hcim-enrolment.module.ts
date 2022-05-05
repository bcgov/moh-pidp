import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { HcimEnrolmentRoutingModule } from './hcim-enrolment-routing.module';
import { HcimEnrolmentPage } from './hcim-enrolment.page';

@NgModule({
  declarations: [HcimEnrolmentPage],
  imports: [HcimEnrolmentRoutingModule, SharedModule],
})
export class HcimEnrolmentModule {}
