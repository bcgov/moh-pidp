import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { MsTeamsClinicMemberRoutingModule } from './ms-teams-clinic-member-routing.module';
import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';

@NgModule({
  declarations: [MsTeamsClinicMemberPage],
  imports: [MsTeamsClinicMemberRoutingModule, SharedModule],
})
export class MsTeamsClinicMemberModule {}
