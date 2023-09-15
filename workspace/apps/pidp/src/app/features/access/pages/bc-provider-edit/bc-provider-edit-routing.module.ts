import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { BcProviderEditPage } from './bc-provider-edit.page';

const routes: Routes = [
  {
    path: '',
    canActivate: [SetDashboardTitleGuard],
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
