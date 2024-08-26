import { Routes } from '@angular/router';

import { ImmsbcPage } from './immsbc.page';
import { ImmsbcResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsbcPage,
    resolve: {
      ImmsbcStatusCode: ImmsbcResolver,
    },
    data: {
      title: 'ImmsBC',
      routes: {
        root: '../../',
      },
    },
  },
];
