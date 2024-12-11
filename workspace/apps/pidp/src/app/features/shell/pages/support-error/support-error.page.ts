import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

import {
  AnchorDirective,
  DashboardHeaderComponent,
  DashboardHeaderConfig,
} from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { ShellRoutes } from '../../shell.routes';
import { FeedbackButtonComponent } from "../../../../shared/components/feedback-button/feedback-button.component";

@Component({
  selector: 'app-support-error',
  templateUrl: './support-error.page.html',
  styleUrls: ['./support-error.page.scss'],
  standalone: true,
  imports: [AnchorDirective, DashboardHeaderComponent, MatButtonModule, FeedbackButtonComponent],
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
    this.router.navigate([ShellRoutes.BASE_PATH]);
  }
}
