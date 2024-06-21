import { Routes } from '@angular/router';

import { setDashboardTitleGuard } from '@pidp/presentation';

import { ProvincialAttachmentSystemPage } from './provincial-attachment-system.page';
import { provincialAttachmentSystemResolver } from './provincial-attachment-system.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ProvincialAttachmentSystemPage,
    canActivate: [setDashboardTitleGuard],
    resolve: {
      provincialAttachmentSystemStatusCode: provincialAttachmentSystemResolver,
    },
    data: {
      setDashboardTitleGuard: {
        titleText: 'Provincial Attachment System',
        titleDescriptionText: '',
      },
    },
  },
];
