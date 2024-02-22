import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { EndorsementsPage } from './endorsements.page';

export const routes: Routes = [
  {
    path: '',
    component: EndorsementsPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'Welcome to your endorsements',
        titleDescriptionText: '',
      },
    },
  },
];
