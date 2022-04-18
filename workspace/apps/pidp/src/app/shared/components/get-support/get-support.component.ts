import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
  selector: 'app-get-support',
  templateUrl: './get-support.component.html',
  styleUrls: ['./get-support.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GetSupportComponent {
  public supports: { name: string; email: string }[];

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.supports = [
      {
        name: 'Provider Identity Portal',
        email: this.config.emails.providerIdentitySupport,
      },
      {
        name: 'Special Authority eForms',
        email: this.config.emails.specialAuthoritySupport,
      },
      {
        name: 'HCIMWeb Enrolment',
        email: this.config.emails.hcimWebSupportEmail,
      },
    ];
  }
}
