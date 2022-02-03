import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { BcscUserInfoComponent } from './components/bcsc-user-info/bcsc-user-info.component';
import { PersonalInformationRoutingModule } from './personal-information-routing.module';
import { PersonalInformationPage } from './personal-information.page';

@NgModule({
  declarations: [PersonalInformationPage, BcscUserInfoComponent],
  imports: [PersonalInformationRoutingModule, SharedModule],
})
export class PersonalInformationModule {}
