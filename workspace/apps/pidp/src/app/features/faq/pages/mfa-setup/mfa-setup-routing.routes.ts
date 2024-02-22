import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { MfaSetupPage } from './mfa-setup.page';

export const routes: Routes = [
  {
    path: '',
    component: MfaSetupPage,
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
