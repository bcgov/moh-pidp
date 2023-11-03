import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { BcProviderEditPage } from './bc-provider-edit.page';
import { highAssuranceCredentialGuard } from '@app/features/auth/guards/high-assurance-credential.guard';

const routes: Routes = [
  {
    path: '',
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

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BcProviderEditRoutingModule {}
