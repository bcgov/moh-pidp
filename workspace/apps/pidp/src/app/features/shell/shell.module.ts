import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { FaqModule } from '../faq/faq.module';
import { PortalDashboardComponent } from './components/portal-dashboard/portal-dashboard.component';
import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  declarations: [PortalDashboardComponent],
  imports: [ShellRoutingModule, DashboardModule, CommonModule, FaqModule],
})
export class ShellModule {}
