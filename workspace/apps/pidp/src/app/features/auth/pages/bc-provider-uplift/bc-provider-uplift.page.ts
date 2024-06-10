import { NgOptimizedImage } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthRoutes } from '../../auth.routes';
import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-bc-provider-uplift',
  templateUrl: './bc-provider-uplift.page.html',
  styleUrls: ['./bc-provider-uplift.page.scss'],
  standalone: true,
  imports: [
    AnchorDirective,
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    NgOptimizedImage,
  ],
})
export class BcProviderUpliftPage {
  public bcscMobileSetupUrl: string;
  public logoutRedirectUrl: string;
  public loginRedirectUrl: string;
  public providerIdentitySupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private authService: AuthService,
  ) {
    this.bcscMobileSetupUrl = this.config.urls.bcscMobileSetup;
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.loginRedirectUrl = `${
      this.config.applicationUrl + AuthRoutes.routePath(AuthRoutes.AUTO_LOGIN)
    }?idp_hint=${IdentityProvider.BCSC}`;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
  }

  public onLogin(): void {
    this.authService.logout(this.loginRedirectUrl);
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }
}
