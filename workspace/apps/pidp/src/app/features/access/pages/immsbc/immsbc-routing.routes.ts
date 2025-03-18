import { Routes } from '@angular/router';

import { AccessRoutes } from '../../access.routes';
import { B2bInformationPageComponent } from '../b2b-information-page/b2b-information-page.component';
import { ImmsBCPage } from './immsbc.page';
import { immsBCResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: B2bInformationPageComponent,
    resolve: {
      immsBCStatusCode: immsBCResolver,
    },
    data: {
      title: 'Bring your own Account',
      routes: {
        root: '../../',
      },
    },
  },
  {
    path: AccessRoutes.IMMSBC,
    component: ImmsBCPage,
    data: {
      title: 'Immunizations BC and OneHealthID',
      routes: {
        root: '../../',
      },
    },
  },
];
