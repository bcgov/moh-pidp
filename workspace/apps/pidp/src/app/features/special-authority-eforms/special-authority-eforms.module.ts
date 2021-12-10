import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';

import { SpecialAuthorityEformsRoutingModule } from './special-authority-eforms-routing.module';
import { SpecialAuthorityEformsComponent } from './special-authority-eforms.component';

@NgModule({
  declarations: [SpecialAuthorityEformsComponent],
  imports: [SpecialAuthorityEformsRoutingModule, SharedModule],
})
export class SpecialAuthorityEformsModule {}
