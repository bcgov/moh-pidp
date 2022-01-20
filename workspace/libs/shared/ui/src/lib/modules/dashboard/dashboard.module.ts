import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';

import { BcGovLogoComponent } from '../../components';
import { NgxProgressBarModule } from '../../modules/ngx-progress-bar/ngx-progress-bar.module';
import { ViewportService } from '../../services/viewport.service';
import { DashboardHeaderComponent } from './components/dashboard-header/dashboard-header.component';
import { DashboardMenuComponent } from './components/dashboard-menu/dashboard-menu.component';
import { DashboardRouteMenuItemComponent } from './components/dashboard-route-menu-item/dashboard-route-menu-item.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    BcGovLogoComponent,
    DashboardHeaderComponent,
    DashboardMenuComponent,
    DashboardRouteMenuItemComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    MatTooltipModule,
    MatSidenavModule,
    NgxProgressBarModule,
  ],
  exports: [DashboardComponent, DashboardHeaderComponent],
  providers: [ViewportService],
})
export class DashboardModule {}
