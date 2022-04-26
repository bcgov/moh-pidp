import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimAccountTransferPage } from './hcim-account-transfer.page';
import { HcimAccountTransferResolver } from './hcim-account-transfer.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimAccountTransferPage,
    resolve: {
      hcimAccountTransferStatusCode: HcimAccountTransferResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HcimAccountTransferRoutingModule {}
