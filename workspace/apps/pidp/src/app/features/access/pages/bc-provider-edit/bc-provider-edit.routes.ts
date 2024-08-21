import { Routes } from '@angular/router';

import { bcProviderEditResolver } from '@app/features/auth/resolvers/bc-provider-edit.resolver';

import { BcProviderEditPage } from './bc-provider-edit.page';

export const routes: Routes = [
  {
    path: '',
    resolve: {
      hasCompletedBCProvider: bcProviderEditResolver,
    },
    component: BcProviderEditPage,
  },
];
