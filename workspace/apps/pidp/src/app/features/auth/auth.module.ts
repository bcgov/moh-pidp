import { NgModule } from '@angular/core';

import { DashboardModule } from '@bcgov/shared/ui';

import { SharedModule } from '@shared/shared.module';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './pages/login/login.component';

@NgModule({
  declarations: [LoginComponent],
  imports: [AuthRoutingModule, DashboardModule, SharedModule],
})
export class AuthModule {}
