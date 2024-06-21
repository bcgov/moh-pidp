import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';
import { HelpPage } from './help.page';


export const routes: Routes = [
  {
    path: '',
    component: HelpPage,
    canActivate: [setDashboardTitleGuard],
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
      setDashboardTitleGuard: {
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
