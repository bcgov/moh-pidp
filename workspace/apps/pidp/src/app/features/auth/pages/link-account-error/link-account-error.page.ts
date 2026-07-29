import { NgOptimizedImage, NgTemplateOutlet } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AnchorDirective,
  InjectViewportCssClassDirective,
  LayoutHeaderFooterComponent,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { DiscoveryStatus } from '@app/core/party/discovery-resource.service';
import { ShellRoutes } from '@app/features/shell/shell.routes';
import { NeedHelpComponent } from '@app/shared/components/need-help/need-help.component';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-link-account-error',
  templateUrl: './link-account-error.page.html',
  styleUrls: ['./link-account-error.page.scss'],
  imports: [
    AnchorDirective,
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    NgOptimizedImage,
    NgTemplateOutlet,
    NeedHelpComponent
],
})
export class LinkAccountErrorPage implements OnInit {
  private readonly config = inject<AppConfig>(APP_CONFIG);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);

  public logoutRedirectUrl: string;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;
  public activeLayout = '';
  public status = '';
  public DiscoveryStatus = DiscoveryStatus;

  public constructor() {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.additionalSupportPhone = this.config.phones.additionalSupport;
    this.route.queryParams.subscribe((params) => {
      this.status = params['status'];
    });
  }

  public onBack(): void {
    this.router.navigate([ShellRoutes.BASE_PATH]);
  }

  public onLogout(): void {
    this.authService.logout(this.logoutRedirectUrl);
  }

  public ngOnInit(): void {
    this.setLayout(this.status);
  }

  private setLayout(activeLayout: string): void {
    if (this.activeLayout !== activeLayout) {
      this.activeLayout = activeLayout;
    }
  }
}
