import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';

import { NgxProgressBarModule } from '@bcgov/shared/ui';

import { DashboardHeaderComponent } from './components/dashboard-header/dashboard-header.component';
import { DashboardMenuComponent } from './components/dashboard-menu/dashboard-menu.component';
import { DashboardRouteMenuItemComponent } from './components/dashboard-route-menu-item/dashboard-route-menu-item.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    DashboardHeaderComponent,
    DashboardMenuComponent,
    DashboardRouteMenuItemComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    RouterModule,
    NgxProgressBarModule,
  ],
  exports: [DashboardHeaderComponent, DashboardComponent],
})
export class DashboardModule {}
