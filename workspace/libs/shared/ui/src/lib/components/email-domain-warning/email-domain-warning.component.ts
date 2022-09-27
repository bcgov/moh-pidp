import { Component, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'ui-email-domain-warning',
  templateUrl: './email-domain-warning.component.html',
  styleUrls: ['./email-domain-warning.component.scss'],
})
export class EmailDomainWarningComponent implements OnChanges {
  @Input() public inputEmail: string;
  @Input() public commonDomains: string[];

  public showWarning: boolean;

  public constructor() {
    this.inputEmail = '';
    this.commonDomains = [];
    this.showWarning = false;
  }

  public ngOnChanges(): void {
    const inputDomain = this.getDomain(this.inputEmail);

    this.showWarning = inputDomain
      ? !this.commonDomains.includes(inputDomain)
      : false;
  }

  private getDomain(email: string): string {
    const atIndex = email?.lastIndexOf('@') ?? -1;

    if (atIndex === -1) {
      return '';
    }

    return email.substring(atIndex + 1);
  }
}
