import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutingModule } from './shell-routing.module';
import { SupportErrorPage } from './pages/support-error/support-error.page';

@NgModule({
  declarations: [PortalDashboardComponent, SupportErrorPage],
  imports: [ShellRoutingModule, DashboardModule, CommonModule],
})
export class ShellModule {}
