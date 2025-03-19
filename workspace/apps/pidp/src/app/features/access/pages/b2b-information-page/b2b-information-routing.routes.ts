import { Routes } from '@angular/router';

import { B2bInformationPageComponent } from '../b2b-information-page/b2b-information-page.component';

export const routes: Routes = [
  {
    path: '',
    component: B2bInformationPageComponent,
    data: {
      title: 'Bring your own Account',
      routes: {
        root: '../../',
      },
    },
  },
];
