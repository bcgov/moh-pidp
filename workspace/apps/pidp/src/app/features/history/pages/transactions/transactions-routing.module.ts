import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { TransactionsPage } from './transactions.page';

const routes: Routes = [
  {
    path: '',
    component: TransactionsPage,
    canActivate: [SetDashboardTitleGuard],
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
export class TransactionsRoutingModule {}
