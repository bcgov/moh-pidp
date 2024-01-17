import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { PrescriptionRefillEformsRoutingModule } from './prescription-refill-eforms-routing.module';
import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';

@NgModule({
  imports: [
    PrescriptionRefillEformsRoutingModule,
    SharedModule,
    AccessModule,
    PrescriptionRefillEformsPage,
  ],
})
export class PrescriptionRefillEformsModule {}
