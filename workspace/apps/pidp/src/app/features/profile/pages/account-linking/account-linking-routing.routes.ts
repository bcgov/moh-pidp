import { Routes } from '@angular/router';

import { AccountLinkingPage } from './account-linking.page';
import { accountLinkingResolver } from './account-linking.resolver';
import { AccountLinkingRoutes } from './account-linking-routes';

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
  {
    path: AccountLinkingRoutes.EXTERNAL_GUEST_INVITATION,
    loadChildren: (): Promise<Routes> =>
      import('./components/external-guest-invitation-routing.routes').then((m) => m.routes),
  }
];
