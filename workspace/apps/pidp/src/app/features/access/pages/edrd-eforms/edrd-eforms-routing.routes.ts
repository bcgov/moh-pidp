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
    },
  },
];
