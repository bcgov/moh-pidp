import { Routes } from '@angular/router';

import { MfaSetupPage } from './mfa-setup.page';

export const routes: Routes = [
  {
    path: '',
    component: MfaSetupPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
