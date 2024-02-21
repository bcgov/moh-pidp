import { Routes } from '@angular/router';

import { FacilityDetailsPage } from './facility-details.page';

export const routes: Routes = [
  {
    path: '',
    component: FacilityDetailsPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
