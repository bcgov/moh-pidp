import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';

import { Observable, map } from 'rxjs';

import {
  DashboardComponent,
  DashboardHeaderTheme,
  DashboardImageComponent,
  IDashboard,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AdminRoutes } from '@app/features/admin/admin.routes';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [AsyncPipe, DashboardComponent, DashboardImageComponent]
})
export class AdminDashboardComponent implements IDashboard {
  public logoutRedirectUrl: string;
  public username: Observable<string>;
  public headerConfig: {
    theme: DashboardHeaderTheme;
    allowMobileToggle: boolean;
  };
  public brandConfig: { imgSrc: string; imgAlt: string };
  public showMenuItemIcons: boolean;
  public responsiveMenuItems: boolean;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    accessTokenService: AccessTokenService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/${this.config.routes.auth}/${AdminRoutes.BASE_PATH}`;
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));
    this.headerConfig = { theme: 'dark', allowMobileToggle: false };
    this.brandConfig = {
      imgSrc: '/assets/images/pidp-logo-white.svg',
      imgAlt: 'OneHealthID Service Logo',
    };
    this.showMenuItemIcons = true;
    this.responsiveMenuItems = true;
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }
}
