import { Routes } from '@angular/router';

import { PemcodPage } from './pemcod.page';
import { pemcodResolver } from './pemcod.resolver';

export const routes: Routes = [
  {
    path: '',
    component: PemcodPage,
    resolve: {
      pemcodStatusCode: pemcodResolver,
    },
  },
];
