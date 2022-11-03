import { LayoutModule } from '@angular/cdk/layout';
import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@shared/shared.module';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginPageV2Component } from './pages/login-v2/login.page.component';
import { LoginPage } from './pages/login/login.page';
import { SystemCardComponent } from './pages/system-card/system-card.page';

@NgModule({
  declarations: [LoginPage, LoginPageV2Component, SystemCardComponent],
  imports: [AuthRoutingModule, DashboardModule, SharedModule, LayoutModule],
})
export class AuthModule {}
