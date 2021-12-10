import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { PortalRoutingModule } from './portal-routing.module';
import { PortalComponent } from './portal.component';

@NgModule({
  declarations: [PortalComponent],
  imports: [PortalRoutingModule, SharedModule],
})
export class PortalModule {}
