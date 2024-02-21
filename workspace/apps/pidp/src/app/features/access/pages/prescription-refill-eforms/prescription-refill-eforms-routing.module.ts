import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';
import { prescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: PrescriptionRefillEformsPage,
    resolve: {
      prescriptionRefillEformsStatusCode: prescriptionRefillEformsResolver,
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
export class PrescriptionRefillEformsRoutingModule {}