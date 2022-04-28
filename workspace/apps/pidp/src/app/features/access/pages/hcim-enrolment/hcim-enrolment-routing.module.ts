import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimEnrolmentComponent } from './hcim-enrolment.component';

const routes: Routes = [
  {
    path: '',
    component: HcimEnrolmentComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HcimEnrolmentRoutingModule {}
