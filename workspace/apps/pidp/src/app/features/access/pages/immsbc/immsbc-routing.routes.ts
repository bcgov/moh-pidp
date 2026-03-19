import { Routes } from '@angular/router';

import { ImmsbcPage } from './immsbc.page';
import { immsbcResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsbcPage,
    resolve: {
      immsBCStatusCode: immsbcResolver,
    },
  },
];
