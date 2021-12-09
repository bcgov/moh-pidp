import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { CollegeLicenceInformationRoutingModule } from './college-licence-information-routing.module';
import { CollegeLicenceInformationComponent } from './college-licence-information.component';

@NgModule({
  declarations: [CollegeLicenceInformationComponent],
  imports: [CollegeLicenceInformationRoutingModule, SharedModule],
})
export class CollegeLicenceInformationModule {}
