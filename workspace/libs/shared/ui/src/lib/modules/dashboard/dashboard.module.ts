import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { BcGovLogoComponent } from '../../components';
import { NgxProgressBarModule } from '../../modules/ngx-progress-bar/ngx-progress-bar.module';
import { ViewportService } from '../../services/viewport.service';
import { SharedUiModule } from '../../shared-ui.module';
import { DashboardImageComponent, DashboardV2Component } from './components';
import { DashboardHeaderComponent } from './components/dashboard-header/dashboard-header.component';
import { DashboardMenuComponent } from './components/dashboard-menu/dashboard-menu.component';
import { DashboardRouteMenuItemComponent } from './components/dashboard-route-menu-item/dashboard-route-menu-item.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    // TODO attempt to reduce the need to include this in the application by having a default CoreModule in lib
    // NOTE: must be included in the application to allow for
    // the NgProgressHttpModule to attach to the HttpClient
    NgxProgressBarModule,
    SharedUiModule,
    BcGovLogoComponent,
    DashboardHeaderComponent,
    DashboardImageComponent,
    DashboardMenuComponent,
    DashboardRouteMenuItemComponent,
    DashboardComponent,
    DashboardV2Component,
  ],
  exports: [
    DashboardComponent,
    DashboardHeaderComponent,
    DashboardImageComponent,
    DashboardV2Component,
  ],
  providers: [ViewportService],
})
export class DashboardModule {}
