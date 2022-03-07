import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimWebEnrolmentPage } from './hcim-web-enrolment.page';

const routes: Routes = [
  {
    path: '',
    component: HcimWebEnrolmentPage,
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
