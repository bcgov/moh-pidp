import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';

import { DashboardHeaderConfig } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { ShellRoutes } from '../../shell.routes';

@Component({
  selector: 'app-support-error',
  templateUrl: './support-error.page.html',
  styleUrls: ['./support-error.page.scss'],
})
export class SupportErrorPage {
  public headerConfig: DashboardHeaderConfig;
  public providerIdentitySupportEmail: string;

  public constructor(
    private router: Router,
    @Inject(APP_CONFIG) private config: AppConfig
  ) {
    this.headerConfig = { theme: 'light', allowMobileToggle: false };
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
  }

  public onBack(): void {
    this.router.navigate([ShellRoutes.MODULE_PATH]);
  }
}
