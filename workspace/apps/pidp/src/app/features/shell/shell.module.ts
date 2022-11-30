import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PidpDataModelModule } from '@pidp/data-model';
import { PidpPresentationModule } from '@pidp/presentation';

import { DashboardModule } from '@bcgov/shared/ui';

import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  declarations: [PortalDashboardComponent],
  imports: [
    ShellRoutingModule,
    DashboardModule,
    CommonModule,
    PidpDataModelModule,
    PidpPresentationModule,
  ],
})
export class ShellModule {}
