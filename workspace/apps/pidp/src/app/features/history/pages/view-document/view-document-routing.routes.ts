import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { ViewDocumentPage } from './view-document.page';

export const routes: Routes = [
  {
    path: '',
    component: ViewDocumentPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];
