import { Routes } from '@angular/router';

import { AccountLinkingPage } from './account-linking.page';
import { accountLinkingResolver } from './account-linking.resolver';

export const routes: Routes = [
  {
    path: '',
    component: AccountLinkingPage,
    resolve: {
      accountLinkingStatusCode: accountLinkingResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
