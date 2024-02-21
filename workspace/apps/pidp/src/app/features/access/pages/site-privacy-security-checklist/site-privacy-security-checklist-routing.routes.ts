import { Routes } from '@angular/router';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

export const routes: Routes = [
  {
    path: '',
    component: SitePrivacySecurityChecklistPage,
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
