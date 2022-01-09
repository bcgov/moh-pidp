import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { ShellRoutingModule } from './shell-routing.module';

@NgModule({
  declarations: [],
  imports: [ShellRoutingModule, DashboardModule],
})
export class ShellModule {}
