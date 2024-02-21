import { Routes } from '@angular/router';

import { AdministratorInformationPage } from './administrator-information.page';

export const routes: Routes = [
  {
    path: '',
    component: AdministratorInformationPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
