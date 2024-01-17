import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { OrganizationDetailsRoutingModule } from './organization-details-routing.module';
import { OrganizationDetailsPage } from './organization-details.page';

@NgModule({
  imports: [
    OrganizationDetailsRoutingModule,
    SharedModule,
    OrganizationDetailsPage,
  ],
})
export class OrganizationDetailsModule {}
