import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimEnrolmentComponent } from './hcim-enrolment.component';
import { HcimEnrolmentResolver } from './hcim-enrolment.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimEnrolmentComponent,
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
