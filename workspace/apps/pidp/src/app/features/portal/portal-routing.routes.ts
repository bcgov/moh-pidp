import { Routes } from '@angular/router';

import { PortalPage } from './portal.page';

export const routes: Routes = [
  {
    path: '',
    component: PortalPage,
    data: {
      title: 'OneHealthID Service',
    },
  },
];
