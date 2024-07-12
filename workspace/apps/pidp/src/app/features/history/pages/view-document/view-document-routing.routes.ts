import { Routes } from '@angular/router';

import { ViewDocumentPage } from './view-document.page';

export const routes: Routes = [
  {
    path: '',
    component: ViewDocumentPage,
    data: {
      title: 'OneHealthID Service',
    },
  },
];
