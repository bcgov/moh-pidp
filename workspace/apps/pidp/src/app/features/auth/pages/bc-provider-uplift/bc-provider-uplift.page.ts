import { Component, Inject } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthRoutes } from '../../auth.routes';
import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-bc-provider-uplift',
  templateUrl: './bc-provider-uplift.page.html',
  styleUrls: ['./bc-provider-uplift.page.scss'],
})
export class BcProviderUpliftPage {
  public bcscMobileSetupUrl: string;
  public logoutRedirectUrl: string;
  public loginRedirectUrl: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService
  ) {
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.loginRedirectUrl = `${
      this.config.applicationUrl + AuthRoutes.routePath(AuthRoutes.AUTO_LOGIN)
    }?idp_hint=${IdentityProvider.BCSC}`;
  }

  public onLogin(): void {
    this.authService.logout(this.loginRedirectUrl);
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }
}
