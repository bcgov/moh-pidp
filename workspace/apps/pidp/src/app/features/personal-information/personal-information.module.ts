import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { PersonalInformationRoutingModule } from './personal-information-routing.module';
import { PersonalInformationComponent } from './personal-information.component';

@NgModule({
  declarations: [PersonalInformationComponent],
  imports: [PersonalInformationRoutingModule, SharedModule],
})
export class PersonalInformationModule {}
