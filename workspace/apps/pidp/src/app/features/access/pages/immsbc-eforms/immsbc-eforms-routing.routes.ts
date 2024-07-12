import { Routes } from '@angular/router';

import { ImmsBCEformsPage } from './immsbc-eforms.page';
import { immsBCEformsResolver } from './immsbc-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsBCEformsPage,
    resolve: {
      immsBCEformsStatusCode: immsBCEformsResolver,
    },
    data: {
      title: 'Immunization Entry eForm and OneHealthID',
      routes: {
        root: '../../',
      },
    },
  },
];
