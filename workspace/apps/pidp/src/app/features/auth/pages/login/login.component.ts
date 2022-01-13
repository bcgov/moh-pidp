import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DashboardHeaderTheme } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { IdentityProviderEnum } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  public theme: DashboardHeaderTheme;
  public loginCancelled: boolean;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute,
  ) {
    this.theme = 'light';
    this.loginCancelled =
      this.route.snapshot.queryParams.action === 'cancelled';
  }

  public onLogin(): void {
    this.authService.login({
      idpHint: IdentityProviderEnum.BCSC,
      redirectUri: this.config.loginRedirectUrl,
    });
  }

  public onTest(): void {
    this.http
      .get('userinfo')
      .subscribe((response) => console.log('RESPONSE', response));
  }
}
