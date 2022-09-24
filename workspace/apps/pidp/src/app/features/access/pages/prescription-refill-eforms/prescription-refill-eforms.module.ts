import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { PrescriptionRefillEformsRoutingModule } from './prescription-refill-eforms-routing.module';
import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';

@NgModule({
  declarations: [PrescriptionRefillEformsPage],
  imports: [PrescriptionRefillEformsRoutingModule, SharedModule, AccessModule],
})
export class PrescriptionRefillEformsModule {}
