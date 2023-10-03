import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AccessModule } from '../../access.module';
import { ImmsBCEformsRoutingModule } from './immsbc-eforms-routing.module';
import { ImmsBCEformsPage } from './immsbc-eforms.page';

@NgModule({
  declarations: [ImmsBCEformsPage],
  imports: [SharedModule, ImmsBCEformsRoutingModule, AccessModule],
})
export class ImmsBCEformsModule {}
