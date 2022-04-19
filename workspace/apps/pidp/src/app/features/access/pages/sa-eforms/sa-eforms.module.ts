import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { SaEformsRoutingModule } from './sa-eforms-routing.module';
import { SaEformsPage } from './sa-eforms.page';

@NgModule({
  declarations: [SaEformsPage],
  imports: [SaEformsRoutingModule, SharedModule],
})
export class SaEformsModule {}
