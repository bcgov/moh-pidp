import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashboardModule, MaterialModule } from '@bcgov/shared/ui';

import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { SupportErrorPage } from './pages/support-error/support-error.page';
import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  declarations: [PortalDashboardComponent, SupportErrorPage],
  imports: [ShellRoutingModule, DashboardModule, CommonModule, MaterialModule],
})
export class ShellModule {}
