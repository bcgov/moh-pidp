import { NgOptimizedImage } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';

import {
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-link-existing-account',
  templateUrl: './link-existing-account.page.html',
  styleUrls: ['./link-existing-account.page.scss'],
  standalone: true,
  imports: [
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    NgOptimizedImage,
  ],
})
export class LinkExistingAccountPage {
  public logoutRedirectUrl: string;
  public providerIdentitySupport: string;

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private authService: AuthService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
  }

  public onBack(): void {
    this.router.navigate([ShellRoutes.BASE_PATH]);
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }
}
