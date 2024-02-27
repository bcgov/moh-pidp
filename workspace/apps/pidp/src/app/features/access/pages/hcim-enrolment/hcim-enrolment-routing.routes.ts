import { Routes } from '@angular/router';

import { HcimEnrolmentPage } from './hcim-enrolment.page';
import { hcimEnrolmentResolver } from './hcim-enrolment.resolver';

export const routes: Routes = [
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
