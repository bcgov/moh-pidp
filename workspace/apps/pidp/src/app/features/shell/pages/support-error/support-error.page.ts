import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { DashboardHeaderConfig } from '@bcgov/shared/ui';

import { ShellRoutes } from '../../shell.routes';

@Component({
  selector: 'app-support-error',
  templateUrl: './support-error.page.html',
  styleUrls: ['./support-error.page.scss'],
})
export class SupportErrorPage {
  public headerConfig: DashboardHeaderConfig;
  public constructor(private router: Router) {
    this.headerConfig = { theme: 'light', allowMobileToggle: false };
  }

  public onBack(): void {
    this.router.navigate([ShellRoutes.MODULE_PATH]);
  }
}
