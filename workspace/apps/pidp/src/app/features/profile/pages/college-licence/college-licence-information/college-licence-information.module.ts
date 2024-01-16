import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { CollegeLicenceInformationRoutingModule } from './college-licence-information-routing.module';
import { CollegeLicenceInformationPage } from './college-licence-information.page';
import { CollegeLicenceInformationDetailComponent } from './components/college-licence-information-detail.component';

@NgModule({
    imports: [SharedModule, CollegeLicenceInformationRoutingModule, CollegeLicenceInformationPage,
        CollegeLicenceInformationDetailComponent],
})
export class CollegeLicenceInformationModule {}
