import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';
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
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    component: BcProviderApplicationPage,
    data: {
      setDashboardTitleGuard: {
        titleText: 'BC Provider and OneHealthID',
        titleDescriptionText: '',
      },
    },
  },
];
