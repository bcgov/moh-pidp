import { Routes } from '@angular/router';

import { ProviderReportingPortalPage } from './provider-reporting-portal.page';
import { providerReportingPortalResolver } from './provider-reporting-portal.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ProviderReportingPortalPage,
    resolve: {
      providerReportingPortalStatusCode: providerReportingPortalResolver,
    },
    data: {
      title: 'OneHealthID Service',
      routes: {
        root: '../../',
      },
    },
  },
];
