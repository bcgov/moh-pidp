import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProviderReportingPortalPage } from './provider-reporting-portal.page';
import { providerReportingPortalResolver } from './provider-reporting-portal.resolver';

const routes: Routes = [
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

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProviderReportingPortalRoutingModule {}