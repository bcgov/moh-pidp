import { Routes } from '@angular/router';

import { ImmsBCPage } from './immsbc.page';
import { immsBCResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsBCPage,
    resolve: {
      immsBCStatusCode: immsBCResolver,
    },
    data: {
      title: 'Immunizations BC and OneHealthID',
      routes: {
        root: '../../',
      },
    },
  },
];
