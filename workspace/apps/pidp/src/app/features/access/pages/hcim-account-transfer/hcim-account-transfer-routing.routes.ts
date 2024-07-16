import { Routes } from '@angular/router';

import { HcimAccountTransferPage } from './hcim-account-transfer.page';
import { hcimAccountTransferResolver } from './hcim-account-transfer.resolver';

export const routes: Routes = [
  {
    path: '',
    component: HcimAccountTransferPage,
    resolve: {
      hcimAccountTransferStatusCode: hcimAccountTransferResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
