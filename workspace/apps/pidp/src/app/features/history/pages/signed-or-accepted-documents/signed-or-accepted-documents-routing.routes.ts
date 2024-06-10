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
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
