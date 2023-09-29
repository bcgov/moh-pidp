import { Component, Inject, Input } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
  selector: 'app-need-help',
  templateUrl: './need-help.component.html',
  styleUrls: ['./need-help.component.scss'],
})
export class NeedHelpComponent {
  @Input() public customClass: 'need-help-large' | undefined;
  @Input() public showIcon: boolean;
  public showNeedHelp: boolean;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.showIcon = false;
    this.showNeedHelp = false;

    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.additionalSupportPhone = this.config.phones.additionalSupport;
  }

  public getClasses(): string[] {
    const classes = ['need-help'];
    if (this.customClass) {
      classes.push(this.customClass);
      return classes;
    }
    return classes;
  }

  public onShowNeedHelp(): void {
    this.showNeedHelp = !this.showNeedHelp;
  }
}
