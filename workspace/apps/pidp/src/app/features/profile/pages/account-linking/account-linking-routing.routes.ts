import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { AccountLinkingPage } from './account-linking.page';
import { accountLinkingResolver } from './account-linking.resolver';

export const routes: Routes = [
  {
    path: '',
    component: AccountLinkingPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      accountLinkingStatusCode: accountLinkingResolver,
    },
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
