import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HcimwebAccountTransferPage } from './hcimweb-account-transfer.page';
import { HcimwebAccountTransferResolver } from './hcimweb-account-transfer.resolver';

const routes: Routes = [
  {
    path: '',
    component: HcimwebAccountTransferPage,
    resolve: {
      hcimwebAccountTransferStatusCode: HcimwebAccountTransferResolver,
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
export class HcimwebAccountTransferRoutingModule {}
