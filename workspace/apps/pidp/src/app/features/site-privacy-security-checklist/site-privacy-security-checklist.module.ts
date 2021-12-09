import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { SitePrivacySecurityChecklistRoutingModule } from './site-privacy-security-checklist-routing.module';
import { SitePrivacySecurityChecklistComponent } from './site-privacy-security-checklist.component';

@NgModule({
  declarations: [SitePrivacySecurityChecklistComponent],
  imports: [SitePrivacySecurityChecklistRoutingModule, SharedModule],
})
export class SitePrivacySecurityChecklistModule {}
