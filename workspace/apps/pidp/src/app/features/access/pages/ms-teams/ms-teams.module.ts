import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { MsTeamsRoutingModule } from './ms-teams-routing.module';
import { MsTeamsPage } from './ms-teams.page';

@NgModule({
  declarations: [MsTeamsPage],
  imports: [SharedModule, MsTeamsRoutingModule],
})
export class MsTeamsModule {}
