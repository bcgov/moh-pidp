import { Routes } from '@angular/router';

import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';
import { msTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';

export const routes: Routes = [
  {
    path: '',
    component: MsTeamsClinicMemberPage,
    resolve: {
      msTeamsClinicMemberStatusCode: msTeamsClinicMemberResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
