import { NgModule } from '@angular/core';

import { FeatureFlagModule } from '@app/modules/feature-flag/feature-flag.module';
import { LookupModule } from '@app/modules/lookup/lookup.module';

import { SharedModule } from '@shared/shared.module';

import { ProfileCardSummaryContentComponent } from './components/profile-card-summary-content/profile-card-summary-content.component';
import { PortalRoutingModule } from './portal-routing.module';
import { PortalComponent } from './portal.component';

@NgModule({
  declarations: [PortalComponent, ProfileCardSummaryContentComponent],
  imports: [
    PortalRoutingModule,
    SharedModule,
    LookupModule.forChild(),
    FeatureFlagModule,
  ],
})
export class PortalModule {}
