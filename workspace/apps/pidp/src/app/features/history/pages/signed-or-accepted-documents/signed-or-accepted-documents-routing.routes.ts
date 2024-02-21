import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

export const routes: Routes = [
  {
    path: '',
    component: SignedOrAcceptedDocumentsPage,
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
