import { Routes } from '@angular/router';

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
    canActivate: [highAssuranceCredentialGuard],
    component: BcProviderApplicationPage,
  },
];
