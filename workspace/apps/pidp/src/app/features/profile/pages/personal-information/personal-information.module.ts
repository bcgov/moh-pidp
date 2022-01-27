import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { BcscUserInfoComponent } from './components/bcsc-user-info/bcsc-user-info.component';
import { PersonalInformationRoutingModule } from './personal-information-routing.module';
import { PersonalInformationComponent } from './personal-information.component';

@NgModule({
  declarations: [PersonalInformationComponent, BcscUserInfoComponent],
  imports: [PersonalInformationRoutingModule, SharedModule],
})
export class PersonalInformationModule {}
