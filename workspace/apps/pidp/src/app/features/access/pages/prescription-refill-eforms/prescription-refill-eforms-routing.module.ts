import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';
import { PrescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';

const routes: Routes = [
  {
    path: '',
    component: PrescriptionRefillEformsPage,
    resolve: {
      prescriptionRefillEformsStatusCode: PrescriptionRefillEformsResolver,
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
export class PrescriptionRefillEformsRoutingModule {}
