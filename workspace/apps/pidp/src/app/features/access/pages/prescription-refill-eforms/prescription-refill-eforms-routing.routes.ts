import { Routes } from '@angular/router';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';
import { prescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';

export const routes: Routes = [
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
