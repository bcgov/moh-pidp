import { Routes } from '@angular/router';

import { ImmsbcPage } from './immsbc.page';
import { ImmsbcAddingAccountsPage } from '../immsbc-adding-accounts/immsbc-adding-accounts.page';
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
];
