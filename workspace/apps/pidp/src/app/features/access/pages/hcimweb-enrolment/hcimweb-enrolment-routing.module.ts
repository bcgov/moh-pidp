import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimwebEnrolmentComponent } from './hcimweb-enrolment.component';

const routes: Routes = [
  {
    path: '',
    component: HcimwebEnrolmentComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HcimwebEnrolmentRoutingModule {}
