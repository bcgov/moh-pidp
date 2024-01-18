import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { TransactionsPage } from './transactions.page';

export const routes: Routes = [
  {
    path: '',
    component: TransactionsPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];
