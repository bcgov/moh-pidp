import { AsyncPipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

import { Observable, map } from 'rxjs';

import {
  DashboardHeaderConfig,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  IDashboard,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessRoutes } from '@app/features/access/access.routes';
import { AuthService } from '@app/features/auth/services/auth.service';
import {
  DashboardStateModel,
  PidpStateName,
} from '@app/features/portal/models/state.model';

import { AppStateService } from '../../services/app-state.service';
import { DashboardStateService } from '../../services/dashboard-state-service.service';
import { NavMenuComponent } from '../navbar-menu/nav-menu';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  standalone: true,
  imports: [AsyncPipe, NavMenuComponent],
})
export class PortalDashboardComponent implements IDashboard, OnInit {
  public logoutRedirectUrl: string;
  public headerConfig: DashboardHeaderConfig;
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;
  public menuItems: DashboardMenuItem[];
  public providerIdentitySupport: string;

  public dashboardState$: Observable<DashboardStateModel>;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private dashboardStateService: DashboardStateService,
    private stateService: AppStateService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}`;
    this.headerConfig = { theme: 'light', allowMobileToggle: true };
    this.brandConfig = {
      imgSrc: '/assets/images/pidp-logo-white.svg',
      imgAlt: 'OneHealthID Service Logo',
    };
    this.showMenuItemIcons = true;
    this.responsiveMenuItems = false;
    this.menuItems = this.createMenuItems();
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;

    this.dashboardState$ = this.stateService.stateBroadcast$.pipe(
      map((state) => {
        const dashboardNamedState = state.all.find(
          (x) => x.stateName === PidpStateName.dashboard,
        );
        if (!dashboardNamedState) {
          throw 'dashboard state not found';
        }
        const dashboardState = dashboardNamedState as DashboardStateModel;
        return dashboardState;
      }),
    );
  }
  public ngOnInit(): void {
    this.dashboardStateService.refreshDashboardState();
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }

  private createMenuItems(): DashboardMenuItem[] {
    const linkActiveOptions = {
      matrixParams: 'exact',
      queryParams: 'exact',
      paths: 'exact',
      fragment: 'exact',
    } as IsActiveMatchOptions;
    return [
      new DashboardRouteMenuItem(
        'Access',
        {
          commands: [AccessRoutes.BASE_PATH, AccessRoutes.ACCESS_REQUESTS],
          linkActiveOptions,
        },
        'assignment',
      ),
      new DashboardRouteMenuItem(
        'Help',
        {
          commands: 'help',
          linkActiveOptions,
        },
        'help_outline',
      ),
    ];
  }
}
