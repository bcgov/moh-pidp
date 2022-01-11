import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { WorkAndRoleInformationRoutingModule } from './work-and-role-information-routing.module';
import { WorkAndRoleInformationComponent } from './work-and-role-information.component';

@NgModule({
  declarations: [WorkAndRoleInformationComponent],
  imports: [WorkAndRoleInformationRoutingModule, SharedModule],
})
export class WorkAndRoleInformationModule {}
