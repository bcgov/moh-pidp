import { Routes } from '@angular/router';

import { OrganizationDetailsPage } from './organization-details.page';

export const routes: Routes = [
  {
    path: '',
    component: OrganizationDetailsPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
