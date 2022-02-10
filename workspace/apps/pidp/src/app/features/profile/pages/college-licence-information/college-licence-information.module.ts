import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { CollegeLicenceInformationRoutingModule } from './college-licence-information-routing.module';
import { CollegeLicenceInformationPage } from './college-licence-information.page';

@NgModule({
  declarations: [CollegeLicenceInformationPage],
  imports: [CollegeLicenceInformationRoutingModule, SharedModule],
})
export class CollegeLicenceInformationModule {}
