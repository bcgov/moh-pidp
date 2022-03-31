import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

import { Observable, map } from 'rxjs';

import {
  DashboardHeaderConfig,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  IDashboard,
} from '@bcgov/shared/ui';
import { ArrayUtils } from '@bcgov/shared/utils';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { PortalRoutes } from '@app/features/portal/portal.routes';
import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PortalDashboardComponent implements IDashboard {
  public logoutRedirectUrl: string;
  public username: Observable<string>;
  public headerConfig: DashboardHeaderConfig;
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;
  public menuItems: DashboardMenuItem[];

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private permissionsService: PermissionsService,
    accessTokenService: AccessTokenService
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}`;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));
    this.headerConfig = { theme: 'light', allowMobileToggle: true };
    this.brandConfig = {
      imgSrc: '/assets/images/pidp-logo-blue.svg',
      imgAlt: 'Provider Identity Portal Logo',
    };
    this.showMenuItemIcons = true;
    this.responsiveMenuItems = false;
    this.menuItems = this.createMenuItems();
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
        'Profile',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'profile' },
          linkActiveOptions,
        },
        'assignment_ind'
      ),
      new DashboardRouteMenuItem(
        'Access to Systems',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'access' },
          linkActiveOptions,
        },
        'assignment'
      ),
      ...ArrayUtils.insertResultIf<any>(
        this.permissionsService.hasRole([
          Role.FEATURE_PIDP_DEMO,
          Role.FEATURE_AMH_DEMO,
        ]),
        () => [
          new DashboardRouteMenuItem(
            'Training',
            {
              commands: PortalRoutes.MODULE_PATH,
              extras: { fragment: 'training' },
              linkActiveOptions,
            },
            'school'
          ),
        ]
      ),
      new DashboardRouteMenuItem(
        'Your Documents',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'documents' },
          linkActiveOptions,
        },
        'restore'
      ),
      new DashboardRouteMenuItem(
        'Get Support',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'support' },
          linkActiveOptions,
        },
        'help_outline'
      ),
    ];
  }
}
