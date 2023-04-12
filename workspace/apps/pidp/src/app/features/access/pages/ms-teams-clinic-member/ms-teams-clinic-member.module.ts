import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { MsTeamsClinicMemberRoutingModule } from './ms-teams-clinic-member-routing.module';
import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';

@NgModule({
  declarations: [MsTeamsClinicMemberPage],
  imports: [
    MsTeamsClinicMemberRoutingModule,
    SharedModule,
    AccessModule,
    FormsModule,
  ],
})
export class MsTeamsClinicMemberModule {}
