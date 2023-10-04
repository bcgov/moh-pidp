import { Component, Inject } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
  selector: 'app-need-help',
  templateUrl: './need-help.component.html',
  styleUrls: ['./need-help.component.scss'],
})
export class NeedHelpComponent {
  public showNeedHelp: boolean;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.showNeedHelp = false;
    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.additionalSupportPhone = this.config.phones.additionalSupport;
  }

  public onShowNeedHelp(): void {
    this.showNeedHelp = !this.showNeedHelp;
  }
}
