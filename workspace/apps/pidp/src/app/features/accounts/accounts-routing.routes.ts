import { Routes } from '@angular/router';

import { AccountsRoutes } from './accounts.routes';

export const routes: Routes = [
  {
    path: AccountsRoutes.ACCOUNT_LINKING,
    loadChildren: (): Promise<Routes> =>
      import('./pages/account-linking/account-linking-routing.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccountsRoutes.BC_PROVIDER_APPLICATION,
    loadChildren: (): Promise<Routes> =>
      import(
        './pages/bc-provider-application/bc-provider-application.route'
      ).then((m) => m.routes),
  },
  {
    path: AccountsRoutes.BC_PROVIDER_EDIT,
    loadChildren: (): Promise<Routes> =>
      import('./pages/bc-provider-edit/bc-provider-edit.routes').then(
        (m) => m.routes,
      ),
  },
  {
    path: AccountsRoutes.EXTERNAL_ACCOUNTS,
    loadChildren: (): Promise<Routes> =>
      import('./pages/external-accounts/external-accounts-routing.routes').then(
        (m) => m.routes,
      ),
  },
];
