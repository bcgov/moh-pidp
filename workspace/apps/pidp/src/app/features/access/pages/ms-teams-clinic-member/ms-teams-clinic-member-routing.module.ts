import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';
import { msTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';

const routes: Routes = [
  {
    path: '',
    component: MsTeamsClinicMemberPage,
    resolve: {
      msTeamsClinicMemberStatusCode: msTeamsClinicMemberResolver,
    },
    canActivate: [setDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'MS Teams Clinic Member',
        titleDescriptionText: 'Stay connected with team members',
      },
      title: 'OneHealthID Service',
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
export class MsTeamsClinicMemberRoutingModule {}
