import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
  selector: 'app-get-support',
  templateUrl: './get-support.component.html',
  styleUrls: ['./get-support.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GetSupportComponent {
  public providerIdentitySupportEmail: string;
  public specialAuthoritySupportEmail: string;

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.providerIdentitySupportEmail =
      this.config.emails.providerIdentitySupport;
    this.specialAuthoritySupportEmail =
      this.config.emails.specialAuthoritySupport;
  }
}
