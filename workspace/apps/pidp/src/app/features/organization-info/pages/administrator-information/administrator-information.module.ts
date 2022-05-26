import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AdministratorInformationRoutingModule } from './administrator-information-routing.module';
import { AdministratorInformationPage } from './administrator-information.page';

@NgModule({
  declarations: [AdministratorInformationPage],
  imports: [AdministratorInformationRoutingModule, SharedModule],
})
export class AdministratorInformationModule {}
