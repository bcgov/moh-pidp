import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { HcimAccountTransferPage } from './hcim-account-transfer.page';
import { hcimAccountTransferResolver } from './hcim-account-transfer.resolver';

export const routes: Routes = [
  {
    path: '',
    component: HcimAccountTransferPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      hcimAccountTransferStatusCode: hcimAccountTransferResolver,
    },
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
