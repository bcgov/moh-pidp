import { NgClass, NgIf } from '@angular/common';
import { Component, Inject, Input } from '@angular/core';

import { AnchorDirective, ExpansionPanelComponent } from '@bcgov/shared/ui';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Component({
    selector: 'app-need-help',
    templateUrl: './need-help.component.html',
    styleUrls: ['./need-help.component.scss'],
    imports: [AnchorDirective, ExpansionPanelComponent, NgClass, NgIf]
})
export class NeedHelpComponent {
  @Input() public showIcon: boolean;
  public showNeedHelp: boolean;
  public providerIdentitySupport: string;
  public additionalSupportPhone: string;

  public get customClasses(): string[] {
    return this._customClasses;
  }

  private _customClasses: string[] = ['need-help'];

  @Input() public set customClass(value: 'need-help-large' | undefined) {
    if (value && this._customClasses.length === 1) {
      this._customClasses.push(value);
    }
  }

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.showIcon = false;
    this.showNeedHelp = false;

    this.providerIdentitySupport = this.config.emails.providerIdentitySupport;
    this.additionalSupportPhone = this.config.phones.additionalSupport;
  }

  public onShowNeedHelp(): void {
    this.showNeedHelp = !this.showNeedHelp;
  }
}
