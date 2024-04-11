import { Routes } from '@angular/router';

import { EdrdEformsPage } from './edrd-eforms.page';
import { edrdEformsResolver } from './edrd-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: EdrdEformsPage,
    resolve: {
      edrdEformsStatusCode: edrdEformsResolver,
    },
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
