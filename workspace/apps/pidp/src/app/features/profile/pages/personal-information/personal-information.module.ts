import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { UserInfoComponent } from './components/user-info/user-info.component';
import { PersonalInformationRoutingModule } from './personal-information-routing.module';
import { PersonalInformationPage } from './personal-information.page';

@NgModule({
    imports: [PersonalInformationRoutingModule, SharedModule, PersonalInformationPage, UserInfoComponent],
})
export class PersonalInformationModule {}
