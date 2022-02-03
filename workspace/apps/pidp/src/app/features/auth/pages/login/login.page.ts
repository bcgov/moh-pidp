import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { IdentityProviderEnum } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage {
  public title: string;
  public loginCancelled: boolean;
  public bcscSupportUrl: string;
  public bcscMobileSetupUrl: string;
  public specialAuthorityUrl: string;
  public providerIdentitySupportEmail: string;
  public specialAuthoritySupportEmail: string;
  public idpHint: IdentityProviderEnum;

  public IdentityProviderEnum = IdentityProviderEnum;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    const routeSnapshot = this.route.snapshot;

    this.title = routeSnapshot.data.title;
    this.loginCancelled = routeSnapshot.queryParams.action === 'cancelled';
    this.bcscSupportUrl = this.config.urls.bcscSupport;
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.specialAuthorityUrl = this.config.urls.specialAuthority;
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
    this.specialAuthoritySupportEmail =
      this.config.emails.specialAuthoritySupport;
    this.idpHint = routeSnapshot.data.idpHint;
  }

  public onLogin(): void {
    this.authService.login({
      idpHint: this.route.snapshot.data.idpHint,
      redirectUri: this.config.applicationUrl,
    });
  }
}
