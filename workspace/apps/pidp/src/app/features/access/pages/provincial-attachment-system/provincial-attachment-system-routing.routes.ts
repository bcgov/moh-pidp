import { Routes } from '@angular/router';

import { ProvincialAttachmentSystemPage } from './provincial-attachment-system.page';
import { provincialAttachmentSystemResolver } from './provincial-attachment-system.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ProvincialAttachmentSystemPage,
    resolve: {
      provincialAttachmentSystemStatusCode: provincialAttachmentSystemResolver,
    },
  },
];
