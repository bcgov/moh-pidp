import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';
import { msTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';

export const routes: Routes = [
  {
    path: '',
    component: MsTeamsPrivacyOfficerPage,
    resolve: {
      msTeamsPrivacyOfficerStatusCode: msTeamsPrivacyOfficerResolver,
    },
    canActivate: [setDashboardTitleGuard],
    data: {
      setDashboardTitleGuard: {
        titleText: 'MS Teams Privacy Officer',
        titleDescriptionText:
          'Here you can add your clinic information and access MS Teams',
      },
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
