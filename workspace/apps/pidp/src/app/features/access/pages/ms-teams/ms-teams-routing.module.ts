import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { MsTeamsPage } from './ms-teams.page';
import { MsTeamsResolver } from './ms-teams.resolver';

const routes: Routes = [
  {
    path: '',
    component: MsTeamsPage,
    resolve: {
      msTeamsStatusCode: MsTeamsResolver,
    },
    canActivate: [SetDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'MS Teams Enrolment',
        titleDescriptionText:
          'Here you can add view and edit your MS teams members',
      },
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
export class MsTeamsRoutingModule {}
