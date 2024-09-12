import { Routes } from '@angular/router';

import { TransactionsPage } from './transactions.page';

export const routes: Routes = [
  {
    path: '',
    component: TransactionsPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
