import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { WorkAndRoleInformationRoutingModule } from './work-and-role-information-routing.module';
import { WorkAndRoleInformationPage } from './work-and-role-information.page';

@NgModule({
  imports: [
    WorkAndRoleInformationRoutingModule,
    SharedModule,
    WorkAndRoleInformationPage,
  ],
})
export class WorkAndRoleInformationModule {}
