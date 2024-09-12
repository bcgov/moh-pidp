import { Routes } from '@angular/router';

import { bcProviderCompletedResolver } from '@app/features/auth/resolvers/bc-provider-completed.resolver';

import { BcProviderApplicationPage } from './bc-provider-application.page';
import { bcProviderApplicationResolver } from './bc-provider-application.resolver';

export const routes: Routes = [
  {
    path: '',
    resolve: {
      bcProviderApplicationStatusCode: bcProviderApplicationResolver,
      hasCompletedBCProvider: bcProviderCompletedResolver,
    },
    component: BcProviderApplicationPage,
  },
];
