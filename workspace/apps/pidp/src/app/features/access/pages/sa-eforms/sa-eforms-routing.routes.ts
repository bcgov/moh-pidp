import { Routes } from '@angular/router';

import { SaEformsPage } from './sa-eforms.page';
import { saEformsResolver } from './sa-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: SaEformsPage,
    resolve: {
      saEformsStatusCode: saEformsResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
