import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { EdrdEformsRoutingModule } from './edrd-eforms-routing.module';
import { EdrdEformsPage } from './edrd-eforms.page';

@NgModule({
  imports: [
    SharedModule,
    EdrdEformsRoutingModule,
    AccessModule,
    EdrdEformsPage,
  ],
})
export class EdrdEformsModule {}
