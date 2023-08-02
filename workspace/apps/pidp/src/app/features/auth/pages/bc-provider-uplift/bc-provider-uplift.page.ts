import { Component, Inject } from '@angular/core';

import { CookieService } from 'ngx-cookie-service';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-bc-provider-uplift',
  templateUrl: './bc-provider-uplift.page.html',
  styleUrls: ['./bc-provider-uplift.page.scss'],
})
export class BcProviderUpliftPage {
  public bcscMobileSetupUrl: string;
  public logoutRedirectUrl: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private cookieService: CookieService,
    private authService: AuthService
  ) {
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  public onLogin(): void {
    throw new Error('onLogin function not implemented');
  }

  public onBack(): void {
    this.cookieService.delete('bcprovider_aad_userid');
    this.authService.logout(this.logoutRedirectUrl);
  }
}
