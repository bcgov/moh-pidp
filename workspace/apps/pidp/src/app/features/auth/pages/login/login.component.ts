import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

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

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private http: HttpClient
  ) {
    this.theme = 'light';
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
