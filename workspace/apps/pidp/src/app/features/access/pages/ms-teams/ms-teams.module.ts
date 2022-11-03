import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { ClinicMemberFormComponent } from './clinic-member-form.component';
import { MsTeamsRoutingModule } from './ms-teams-routing.module';
import { MsTeamsPage } from './ms-teams.page';

@NgModule({
  declarations: [MsTeamsPage, ClinicMemberFormComponent],
  imports: [SharedModule, MsTeamsRoutingModule, AccessModule],
})
export class MsTeamsModule {}
