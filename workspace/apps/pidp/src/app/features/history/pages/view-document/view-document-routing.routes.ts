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
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
