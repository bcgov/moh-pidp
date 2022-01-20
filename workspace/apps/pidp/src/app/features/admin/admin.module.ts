import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@app/shared/shared.module';

import { AdminRoutingModule } from './admin-routing.module';
import { PartiesComponent } from './pages/parties/parties.component';

@NgModule({
  declarations: [PartiesComponent],
  imports: [AdminRoutingModule, DashboardModule, SharedModule],
})
export class AdminModule {}
