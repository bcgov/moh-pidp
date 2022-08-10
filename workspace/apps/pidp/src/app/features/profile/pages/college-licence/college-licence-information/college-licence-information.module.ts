import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { CollegeLicenceInformationRoutingModule } from './college-licence-information-routing.module';
import { CollegeLicenceInformationPage } from './college-licence-information.page';

@NgModule({
  declarations: [CollegeLicenceInformationPage],
  imports: [SharedModule, CollegeLicenceInformationRoutingModule],
})
export class CollegeLicenceInformationModule {}
