import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';

import { Observable, map } from 'rxjs';

import { IDashboard } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { AuthService } from '@app/features/auth/services/auth.service';

@Component({
  selector: 'app-portal-dashboard',
  templateUrl: './portal-dashboard.component.html',
  styleUrls: ['./portal-dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PortalDashboardComponent implements IDashboard {
  public logoutRedirectUrl: string;
  public username: Observable<string>;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    accessTokenService: AccessTokenService
  ) {
    this.logoutRedirectUrl = `${this.config.loginRedirectUrl}/${this.config.routes.auth}`;
    this.username = accessTokenService
      .decodeToken$()
      .pipe(map((token) => token?.name ?? ''));
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }
}
