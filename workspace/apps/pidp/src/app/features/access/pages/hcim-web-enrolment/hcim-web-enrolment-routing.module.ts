import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimWebEnrolmentComponent } from './hcim-web-enrolment.component';

const routes: Routes = [
  {
    path: '',
    component: HcimWebEnrolmentComponent,
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HcimWebEnrolmentRoutingModule {}
