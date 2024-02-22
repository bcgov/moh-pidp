import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { SaEformsPage } from './sa-eforms.page';
import { saEformsResolver } from './sa-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: SaEformsPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      saEformsStatusCode: saEformsResolver,
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
