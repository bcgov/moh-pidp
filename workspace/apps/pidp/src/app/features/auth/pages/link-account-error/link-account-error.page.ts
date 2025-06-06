import {
  NgIf,
  NgOptimizedImage,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
  NgTemplateOutlet,
} from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
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
  standalone: true,
  imports: [
    AnchorDirective,
    InjectViewportCssClassDirective,
    LayoutHeaderFooterComponent,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    NgOptimizedImage,
    NgIf,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    NgTemplateOutlet,
    NeedHelpComponent,
  ],
})
export class LinkAccountErrorPage implements OnInit {
  public logoutRedirectUrl: string;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;
  public activeLayout = '';
  public status = '';
  public DiscoveryStatus = DiscoveryStatus;

  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authService: AuthService,
  ) {
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
