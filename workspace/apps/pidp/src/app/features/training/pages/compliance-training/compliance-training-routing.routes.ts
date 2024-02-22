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
    },
  },
];
