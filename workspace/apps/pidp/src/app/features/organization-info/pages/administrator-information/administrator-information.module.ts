import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AdministratorInformationRoutingModule } from './administrator-information-routing.module';
import { AdministratorInformationComponent } from './administrator-information.component';

@NgModule({
  declarations: [AdministratorInformationComponent],
  imports: [AdministratorInformationRoutingModule, SharedModule],
})
export class AdministratorInformationModule {}
