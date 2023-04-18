import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';
import { MsTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';

const routes: Routes = [
  {
    path: '',
    component: MsTeamsPrivacyOfficerPage,
    resolve: {
      msTeamsPrivacyOfficerStatusCode: MsTeamsPrivacyOfficerResolver,
    },
    canActivate: [SetDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'MS Teams Privacy Officer',
        titleDescriptionText:
          'Here you can add your clinic information and access MS Teams',
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
export class MsTeamsPrivacyOfficerRoutingModule {}
