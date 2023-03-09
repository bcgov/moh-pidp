import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { ProviderReportingPortalRoutingModule } from './provider-reporting-portal-routing.module';
import { ProviderReportingPortalPage } from './provider-reporting-portal.page';

@NgModule({
  declarations: [ProviderReportingPortalPage],
  imports: [ProviderReportingPortalRoutingModule, SharedModule, AccessModule],
})
export class ProviderReportingPortalModule {}
