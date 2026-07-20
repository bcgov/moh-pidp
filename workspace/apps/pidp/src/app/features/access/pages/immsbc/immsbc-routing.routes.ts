import { Routes } from '@angular/router';

import { ImmsbcPage } from './immsbc.page';
import { ImmsbcAddingAccountsPage } from '../immsbc-adding-accounts/immsbc-adding-accounts.page';
import { ImmsbcAccountAccessChangePage } from '../immsbc-account-access-change/immsbc-account-access-change.page';
import { ImmsbcPasswordResetPage } from '../immsbc-password-reset/immsbc-password-reset.page';
import { immsbcResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsbcPage,
    resolve: {
      immsBCStatusCode: immsbcResolver,
    },
  },
  {
    path: 'adding-accounts',
    component: ImmsbcAddingAccountsPage,
  },
  {
    path: 'account-access-change',
    component: ImmsbcAccountAccessChangePage,
  },
  {
    path: 'password-reset',
    component: ImmsbcPasswordResetPage,
  },
];
