import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { SpecialAuthorityEformsRoutingModule } from './special-authority-eforms-routing.module';
import { SpecialAuthorityEformsPage } from './special-authority-eforms.page';

@NgModule({
  declarations: [SpecialAuthorityEformsPage],
  imports: [SpecialAuthorityEformsRoutingModule, SharedModule],
})
export class SpecialAuthorityEformsModule {}
