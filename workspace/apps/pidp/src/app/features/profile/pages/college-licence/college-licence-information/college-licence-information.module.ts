import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { CollegeLicenceInformationRoutingModule } from './college-licence-information-routing.module';
import { CollegeLicenceInformationV2Page } from './college-licence-information-v2.page';
import { CollegeLicenceInformationPage } from './college-licence-information.page';
import { CollegeLicenceInformationDetailComponent } from './components/college-licence-information-detail.component';

@NgModule({
  declarations: [
    CollegeLicenceInformationPage,
    CollegeLicenceInformationV2Page,
    CollegeLicenceInformationDetailComponent,
  ],
  imports: [SharedModule, CollegeLicenceInformationRoutingModule],
})
export class CollegeLicenceInformationModule {}
