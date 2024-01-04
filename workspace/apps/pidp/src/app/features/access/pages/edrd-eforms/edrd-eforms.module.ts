import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { EdrdEformsRoutingModule } from './edrd-eforms-routing.module';
import { EdrdEformsPage } from './edrd-eforms.page';

@NgModule({
  declarations: [EdrdEformsPage],
  imports: [SharedModule, EdrdEformsRoutingModule, AccessModule],
})
export class EdrdEformsModule {}
