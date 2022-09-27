import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
} from '@angular/core';

import { EMPTY } from 'rxjs';

import { LookupResource } from '@app/modules/lookup/lookup-resource.service';

@Component({
  selector: 'app-email-with-domain-validation',
  templateUrl: './email-with-domain-validation.component.html',
  styleUrls: ['./email-with-domain-validation.component.scss'],
})
export class EmailWithDomainValidationComponent implements OnInit, OnChanges {
  /**
   * @description
   * Email input by the user.
   */
  @Input() public inputEmail: string;

  public showWarning: boolean;
  private commonEmailDomains: string[];

  public constructor(private lookupResource: LookupResource) {
    this.inputEmail = '';
    this.showWarning = false;
    this.commonEmailDomains = [];
  }

  public ngOnChanges(): void {
    const inputDomain = this.inputEmail?.split('@').pop();
    this.showWarning = inputDomain === 'google.ca';
    console.log(inputDomain);
    console.log(this.showWarning);
    // this.showWarning = inputDomain
    //   ? !this.commonEmailDomains.includes(inputDomain)
    //   : false;
  }

  public ngOnInit(): void {
    // this.lookupResource.getCommonEmailDomains().subscribe((emails) => {
    //   emails ? this.commonEmailDomains.push(...emails) : EMPTY;
    // });
  }
}
