import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimEnrolmentPage } from './hcim-enrolment.page';
import { HcimEnrolmentResolver } from './hcim-enrolment.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimEnrolmentPage,
    resolve: {
      hcimEnrolmentStatusCode: HcimEnrolmentResolver,
    },
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
export class HcimEnrolmentRoutingModule {}
