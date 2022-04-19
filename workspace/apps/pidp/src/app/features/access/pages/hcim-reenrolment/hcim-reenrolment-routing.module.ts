import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimReenrolmentPage } from './hcim-reenrolment.page';
import { HcimReenrolmentResolver } from './hcim-reenrolment.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimReenrolmentPage,
    resolve: {
      hcimReenrolmentStatusCode: HcimReenrolmentResolver,
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
export class HcimWebEnrolmentRoutingModule {}
