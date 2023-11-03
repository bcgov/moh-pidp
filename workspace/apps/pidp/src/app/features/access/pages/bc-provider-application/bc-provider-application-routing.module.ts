import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { BcProviderApplicationPage } from './bc-provider-application.page';
import { BcProviderApplicationResolver } from './bc-provider-application.resolver';
import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

const routes: Routes = [
  {
    path: '',
    resolve: {
      bcProviderApplicationStatusCode: BcProviderApplicationResolver,
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

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BcProviderApplicationRoutingModule {}
