import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimReenrolmentPage } from './hcim-reenrolment.page';

const routes: Routes = [
  {
    path: '',
    component: HcimReenrolmentPage,
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
