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
      setDashboardTitleGuard: {
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
