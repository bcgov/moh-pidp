import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { GisRoutingModule } from './gis-routing.module';
import { GisComponent } from './gis.component';

@NgModule({
  declarations: [GisComponent],
  imports: [GisRoutingModule, SharedModule],
})
export class GisModule {}
