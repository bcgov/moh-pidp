import { Routes } from '@angular/router';

import { HelpPage } from './help.page';

export const routes: Routes = [
  {
    path: '',
    component: HelpPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
