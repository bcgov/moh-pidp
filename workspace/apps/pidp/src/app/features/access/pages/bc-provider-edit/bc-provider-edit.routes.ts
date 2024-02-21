import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';
import { bcProviderEditResolver } from '@app/features/auth/resolvers/bc-provider-edit.resolver';

import { BcProviderEditPage } from './bc-provider-edit.page';

export const routes: Routes = [
  {
    path: '',
    resolve: {
      hasCompletedBCProvider: bcProviderEditResolver,
    },
    canActivate: [setDashboardTitleGuard, highAssuranceCredentialGuard],
    component: BcProviderEditPage,
    data: {
      setDashboardTitleGuard: {
        titleText: 'BC Provider and OneHealthID',
        titleDescriptionText: '',
      },
    },
  },
];
