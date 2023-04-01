import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';
import { MsTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';

const routes: Routes = [
  {
    path: '',
    component: MsTeamsClinicMemberPage,
    resolve: {
      msTeamsClinicMemberStatusCode: MsTeamsClinicMemberResolver,
    },
    canActivate: [SetDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'MS Teams Clinic Member',
        titleDescriptionText:
          'Here you sign up for MS Teams once you are endorsed by the clinic Privacy Officer',
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
export class MsTeamsClinicMemberRoutingModule {}
