import { Routes } from '@angular/router';

import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';
import { msTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';

export const routes: Routes = [
  {
    path: '',
    component: MsTeamsPrivacyOfficerPage,
    resolve: {
      msTeamsPrivacyOfficerStatusCode: msTeamsPrivacyOfficerResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
