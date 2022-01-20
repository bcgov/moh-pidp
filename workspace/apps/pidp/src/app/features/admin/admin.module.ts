import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { LookupModule } from '@app/modules/lookup/lookup.module';
import { SharedModule } from '@app/shared/shared.module';

import { AdminRoutingModule } from './admin-routing.module';
import { PartiesComponent } from './pages/parties/parties.component';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';

@NgModule({
  declarations: [PartiesComponent, AdminDashboardComponent],
  imports: [
    AdminRoutingModule,
    DashboardModule,
    SharedModule,
    LookupModule.forChild(),
  ],
})
export class AdminModule {}
