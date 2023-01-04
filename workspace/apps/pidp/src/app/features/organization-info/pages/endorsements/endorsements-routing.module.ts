import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { EndorsementsPage } from './endorsements.page';

const routes: Routes = [
  {
    path: '',
    component: EndorsementsPage,
    canActivate: [SetDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'Welcome to your endorsements',
        titleDescriptionText: '',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EndorsementsRoutingModule {}
