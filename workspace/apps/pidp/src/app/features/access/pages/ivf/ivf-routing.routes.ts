import { Routes } from '@angular/router';

import { IvfPage } from './ivf.page';
import { IvfResolver } from './ivf.resolver';

export const routes: Routes = [
  {
    path: '',
    component: IvfPage,
    resolve: {
      IvfStatusCode: IvfResolver,
    },
    data: {
      title: 'Ivf',
      routes: {
        root: '../../',
      },
    },
  },
];
