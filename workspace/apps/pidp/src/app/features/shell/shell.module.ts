import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  imports: [
    ShellRoutingModule,
    DashboardModule,
    CommonModule,
    PortalDashboardComponent,
  ],
})
export class ShellModule {}
