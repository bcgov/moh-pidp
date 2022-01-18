import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DashboardHeaderTheme } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  public theme: DashboardHeaderTheme;
  public loginCancelled: boolean;
  public bcscSupportUrl: string;
  public pidpSupportEmail: string;
  public idpHint: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.theme = 'light';
    this.loginCancelled =
      this.route.snapshot.queryParams.action === 'cancelled';
    this.bcscSupportUrl = this.config.urls.bcscSupport;
    this.pidpSupportEmail = this.config.emails.support;
    this.idpHint = this.route.snapshot.data.idpHint;
  }

  public onLogin(): void {
    this.authService.login({
      idpHint: this.route.snapshot.data.idpHint,
      redirectUri: this.config.loginRedirectUrl,
    });
  }
}
