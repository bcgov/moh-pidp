import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

import {
  AnchorDirective,
  DashboardHeaderConfig,
  DashboardModule,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { ShellRoutes } from '../../shell.routes';

@Component({
  selector: 'app-support-error',
  templateUrl: './support-error.page.html',
  styleUrls: ['./support-error.page.scss'],
  standalone: true,
  imports: [AnchorDirective, DashboardModule, MatButtonModule],
})
export class SupportErrorPage {
  public headerConfig: DashboardHeaderConfig;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;

  public constructor(
    private router: Router,
    @Inject(APP_CONFIG) private config: AppConfig,
  ) {
    this.headerConfig = { theme: 'light', allowMobileToggle: false };
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.additionalSupportPhone = this.config.phones.additionalSupport;
  }

  public onBack(): void {
    this.router.navigate([ShellRoutes.MODULE_PATH]);
  }
}
