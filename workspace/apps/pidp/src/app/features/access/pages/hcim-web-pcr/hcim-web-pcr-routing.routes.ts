import { Routes } from '@angular/router';

import { HcimWebPcrPage } from './hcim-web-pcr.page';
import { hcimWebPcrResolver } from './hcim-web-pcr.resolver';

export const routes: Routes = [
  {
    path: '',
    component: HcimWebPcrPage,
    resolve: {
      hcimWebPcrStatusCode: hcimWebPcrResolver,
    },
  },
];
