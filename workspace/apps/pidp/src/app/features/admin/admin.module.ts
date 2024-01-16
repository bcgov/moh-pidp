import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { LookupModule } from '@app/modules/lookup/lookup.module';
import { SharedModule } from '@app/shared/shared.module';

import { AdminRoutingModule } from './admin-routing.module';
import { PartiesPage } from './pages/parties/parties.page';
import { AdminDashboardComponent } from './shared/components/admin-dashboard/admin-dashboard.component';

@NgModule({
    imports: [
        AdminRoutingModule,
        DashboardModule,
        SharedModule,
        LookupModule.forChild(),
        AdminDashboardComponent, PartiesPage,
    ],
})
export class AdminModule {}
