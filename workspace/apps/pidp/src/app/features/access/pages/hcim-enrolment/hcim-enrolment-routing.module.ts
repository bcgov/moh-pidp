import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimEnrolmentPage } from './hcim-enrolment.page';
import { hcimEnrolmentResolver } from './hcim-enrolment.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimEnrolmentPage,
    resolve: {
      hcimEnrolmentStatusCode: hcimEnrolmentResolver,
    },
    data: {
      title: 'OneHealthID Service',
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
export class HcimEnrolmentRoutingModule {}
