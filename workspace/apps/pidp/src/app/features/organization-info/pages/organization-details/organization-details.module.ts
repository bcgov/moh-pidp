import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { OrganizationDetailsRoutingModule } from './organization-details-routing.module';
import { OrganizationDetailsComponent } from './organization-details.component';

@NgModule({
  declarations: [OrganizationDetailsComponent],
  imports: [OrganizationDetailsRoutingModule, SharedModule],
})
export class OrganizationDetailsModule {}
