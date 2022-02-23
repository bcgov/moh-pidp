import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';

import { Observable, map } from 'rxjs';

import {
  DashboardHeaderTheme,
  DashboardMenuItem,
  DashboardRouteMenuItem,
  IDashboard,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';
import { PortalRoutes } from '@app/features/portal/portal.routes';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PortalDashboardComponent implements IDashboard {
  public logoutRedirectUrl: string;
  public username: Observable<string>;
  public headerConfig: {
    theme: DashboardHeaderTheme;
    allowMobileToggle: boolean;
  };
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;
  public menuItems: DashboardMenuItem[];

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
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
    return [
      new DashboardRouteMenuItem(
        'Profile',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'profile' },
        },
        'assignment_ind'
      ),
      new DashboardRouteMenuItem(
        'Access to Systems',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'access' },
        },
        'assignment'
      ),
      new DashboardRouteMenuItem(
        'Your Documents',
        {
          commands: PortalRoutes.MODULE_PATH,
          extras: { fragment: 'documents' },
        },
        'restore'
      ),
      new DashboardRouteMenuItem(
        'Get Support',
        {
          commands: PortalRoutes.MODULE_PATH,
        },
        'help_outline'
      ),
    ];
  }
}
