import { NgModule } from '@angular/core';

import { LookupModule } from '@app/modules/lookup/lookup.module';
import { PermissionsModule } from '@app/modules/permissions/permissions.module';

import { SharedModule } from '@shared/shared.module';

import { ProfileCardSummaryContentComponent } from './components/profile-card-summary-content/profile-card-summary-content.component';
import { PortalRoutingModule } from './portal-routing.module';
import { PortalPage } from './portal.page';

@NgModule({
  declarations: [PortalPage, ProfileCardSummaryContentComponent],
  imports: [
    PortalRoutingModule,
    SharedModule,
    LookupModule.forChild(),
    PermissionsModule,
  ],
})
export class PortalModule {}
