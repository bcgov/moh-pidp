import { ClipboardModule } from '@angular/cdk/clipboard';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';

import { LookupModule } from '@app/modules/lookup/lookup.module';
import { PermissionsModule } from '@app/modules/permissions/permissions.module';

import { SharedModule } from '@shared/shared.module';

import { BannerExpansionPanelComponent } from './components/banner-expansion-panel/banner-expansion-panel.component';
import { PortalAlertComponent } from './components/portal-alert/portal-alert.component';
import { PortalCardComponent } from './components/portal-card/portal-card.component';
import { PortalCarouselComponent } from './components/portal-carousel/portal-carousel.component';
import { ProfileCardSummaryContentComponent } from './components/profile-card-summary-content/profile-card-summary-content.component';
import { PortalRoutingModule } from './portal-routing.module';
import { PortalPage } from './portal.page';

@NgModule({
  declarations: [
    PortalPage,
    ProfileCardSummaryContentComponent,
    PortalCardComponent,
    PortalCarouselComponent,
    BannerExpansionPanelComponent,
  ],
  imports: [
    PortalRoutingModule,
    SharedModule,
    LookupModule.forChild(),
    PermissionsModule,
    ClipboardModule,
    PortalAlertComponent,
  ],
  schemas: [
    // This causes the compiler to allow the non-angular swiper html tags.
    // Without this schema, compiling will fail on the swiper tags.
    CUSTOM_ELEMENTS_SCHEMA,
  ],
})
export class PortalModule {}
