import { Routes } from '@angular/router';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

export const routes: Routes = [
  {
    path: '',
    component: SignedOrAcceptedDocumentsPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
