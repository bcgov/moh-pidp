import { Routes } from '@angular/router';

import { InfantRsvEformsPage } from './infant-rsv-eforms.page';
import { infantRsvEformsResolver } from './infant-rsv-eforms.resolver';

export const routes: Routes = [
  {
    path: '',
    component: InfantRsvEformsPage,
    resolve: {
      infantRsvEformsStatusCode: infantRsvEformsResolver,
    },
    data: {
      title: 'Infant RSV Immunization Request eForm and OneHealthID',
      routes: {
        root: '../../',
      },
    },
  },
];
