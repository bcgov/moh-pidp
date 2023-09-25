import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { BcProviderApplicationPage } from './bc-provider-application.page';
import { BcProviderApplicationResolver } from './bc-provider-application.resolver';

const routes: Routes = [
  {
    path: '',
    resolve: {
      bcProviderApplicationStatusCode: BcProviderApplicationResolver,
    },
    canActivate: [SetDashboardTitleGuard],
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
