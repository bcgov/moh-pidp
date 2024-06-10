import { Routes } from '@angular/router';

import { ComplianceTrainingPage } from './compliance-training.page';

export const routes: Routes = [
  {
    path: '',
    component: ComplianceTrainingPage,
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
