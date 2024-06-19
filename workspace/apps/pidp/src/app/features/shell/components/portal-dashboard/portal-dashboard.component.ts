import { AsyncPipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

import { Observable, map } from 'rxjs';

import { DashboardStateModel, PidpStateName } from '@pidp/data-model';
import { AppStateService } from '@pidp/presentation';

import {
  DashboardHeaderConfig,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  IDashboard,
  NavMenuComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { PermissionsService } from '@app/modules/permissions/permissions.service';

import { DashboardStateService } from '../../services/dashboard-state-service.service';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  standalone: true,
  imports: [AsyncPipe, NavMenuComponent],
})
export class PortalDashboardComponent implements IDashboard, OnInit {
  public logoutRedirectUrl: string;
  public username: Observable<string>;
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
    private permissionsService: PermissionsService,
    accessTokenService: AccessTokenService,
    private dashboardStateService: DashboardStateService,
    private stateService: AppStateService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}`;
    this.username = accessTokenService.decodeToken().pipe(
      map((token) => {
        return token?.name ?? '';
      }),
    );
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
          commands:[AccessRoutes.BASE_PATH,AccessRoutes.ACCESS_REQUESTS],
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
