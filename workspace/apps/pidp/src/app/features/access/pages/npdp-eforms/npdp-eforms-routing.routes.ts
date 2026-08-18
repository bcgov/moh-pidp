import { Routes } from '@angular/router';

import { NpdpEformsPage } from './npdp-eforms.page';
import { npdpEformsResolver } from './npdp-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: NpdpEformsPage,
    resolve: {
      npdpEformsStatusCode: npdpEformsResolver,
    },
    data: {
      title: 'Exceptional Coverage: National PharmaCare and Mifepristone/Misoprostol and OneHealthID',
      routes: {
        root: '../../',
      },
    },
  },
];
