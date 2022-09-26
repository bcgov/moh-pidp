import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { EMPTY } from 'rxjs';

import { LookupResource } from '@app/modules/lookup/lookup-resource.service';

@Component({
  selector: 'app-email-with-domain-validation',
  templateUrl: './email-with-domain-validation.component.html',
  styleUrls: ['./email-with-domain-validation.component.scss'],
})
export class EmailWithDomainValidationComponent implements OnInit {
  /**
   * @description
   * Email input by the user.
   */
  @Input() public inputEmail: string;
  /**
   * @description
   * Emits true if email input is found in common email domain list.
   */
  @Output() public domainRecognized: EventEmitter<boolean>;

  public commonEmailDomains: string[];

  public constructor(private lookupResource: LookupResource) {
    this.inputEmail = '';
    this.domainRecognized = new EventEmitter<boolean>();
    this.commonEmailDomains = [];
  }

  public ngOnInit(): void {
    this.lookupResource.getCommonEmailDomains().subscribe((emails) => {
      emails ? this.commonEmailDomains.push(...emails) : EMPTY;
    });
  }
}
