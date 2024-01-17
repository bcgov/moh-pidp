import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { MsTeamsPrivacyOfficerRoutingModule } from './ms-teams-privacy-officer-routing.module';
import { MsTeamsPrivacyOfficerPage } from './ms-teams-privacy-officer.page';

@NgModule({
  imports: [
    SharedModule,
    MsTeamsPrivacyOfficerRoutingModule,
    AccessModule,
    MsTeamsPrivacyOfficerPage,
  ],
})
export class MsTeamsPrivacyOfficerModule {}
