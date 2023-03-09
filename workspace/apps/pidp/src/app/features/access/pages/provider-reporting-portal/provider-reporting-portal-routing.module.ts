import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProviderReportingPortalPage } from './provider-reporting-portal.page';
import { ProviderReportingPortalResolver } from './provider-reporting-portal.resolver';

const routes: Routes = [
  {
    path: '',
    component: ProviderReportingPortalPage,
    resolve: {
      providerReportingPortalStatusCode: ProviderReportingPortalResolver,
    },
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProviderReportingPortalRoutingModule {}
