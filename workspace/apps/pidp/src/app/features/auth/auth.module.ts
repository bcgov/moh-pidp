import { LayoutModule } from '@angular/cdk/layout';
import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@shared/shared.module';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginPage } from './pages/login/login.page';
import { SystemCardComponent } from './pages/system-card/system-card.page';

@NgModule({
  declarations: [LoginPage, SystemCardComponent],
  imports: [AuthRoutingModule, DashboardModule, SharedModule, LayoutModule],
})
export class AuthModule {}
