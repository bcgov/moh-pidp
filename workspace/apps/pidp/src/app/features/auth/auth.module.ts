import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@shared/shared.module';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginPage } from './pages/login/login.page';

@NgModule({
  declarations: [LoginPage],
  imports: [AuthRoutingModule, DashboardModule, SharedModule],
})
export class AuthModule {}
