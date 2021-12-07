import { NgModule } from '@angular/core';

import { DashboardModule } from '../../modules/dashboard/dashboard.module';

import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  declarations: [],
  imports: [ShellRoutingModule, DashboardModule],
})
export class ShellModule {}
