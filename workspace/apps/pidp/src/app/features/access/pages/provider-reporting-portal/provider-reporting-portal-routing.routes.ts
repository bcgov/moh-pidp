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
      setDashboardTitleGuard: {
        titleText: 'Welcome to OneHealthID Service',
        titleDescriptionText:
          'Complete your profile to gain access to the systems you are eligible for',
      },
    },
  },
];
