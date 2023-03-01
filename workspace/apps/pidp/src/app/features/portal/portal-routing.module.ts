import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { PortalPage } from './portal.page';

const routes: Routes = [
  {
    path: '',
    component: PortalPage,
    canActivate: [SetDashboardTitleGuard],
    data: {
      title: 'Provider Identity Portal',
      setDashboardTitleGuard: {
        titleText: 'Welcome to Provider Identity Portal',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PortalRoutingModule {}
