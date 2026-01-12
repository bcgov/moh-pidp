import { Routes } from '@angular/router';

import { IvfPage } from './ivf.page';
import { ivfResolver } from './ivf.resolver';

export const routes: Routes = [
  {
    path: '',
    component: IvfPage,
    resolve: {
      ivfStatusCode: ivfResolver,
    },
  },
];
