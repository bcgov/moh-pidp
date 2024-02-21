import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { PortalPage } from './portal.page';

export const routes: Routes = [
  {
    path: '',
    component: PortalPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
      setDashboardTitleGuard: {
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
