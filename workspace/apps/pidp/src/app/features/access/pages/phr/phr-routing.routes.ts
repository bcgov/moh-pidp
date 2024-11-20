import { Routes } from '@angular/router';

import { PHRPage } from './phr.page';
import { phrResolver } from './phr.resolver';

export const routes: Routes = [
  {
    path: '',
    component: PHRPage,
    resolve: {
      phrStatusCode: phrResolver,
    },
  },
];
