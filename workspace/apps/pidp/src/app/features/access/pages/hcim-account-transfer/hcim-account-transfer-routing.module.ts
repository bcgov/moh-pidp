import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { HcimAccountTransferPage } from './hcim-account-transfer.page';
import { HcimAccountTransferResolver } from './hcim-account-transfer.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimAccountTransferPage,
    canActivate: [SetDashboardTitleGuard],
    resolve: {
      hcimAccountTransferStatusCode: HcimAccountTransferResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: '',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HcimAccountTransferRoutingModule {}
