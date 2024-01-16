import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { SitePrivacySecurityChecklistRoutingModule } from './site-privacy-security-checklist-routing.module';
import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

@NgModule({
    imports: [SitePrivacySecurityChecklistRoutingModule, SharedModule, SitePrivacySecurityChecklistPage],
})
export class SitePrivacySecurityChecklistModule {}
