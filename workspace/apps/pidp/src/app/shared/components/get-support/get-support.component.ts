import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

import { ArrayUtils } from '@bcgov/shared/utils';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AccessSectionStatus } from '@app/features/portal/sections/models/access-status.model';

export type SupportProvided = keyof AccessSectionStatus;

interface SupportProps {
  name: string;
  email: string;
}

@Component({
  selector: 'app-get-support',
  templateUrl: './get-support.component.html',
  styleUrls: ['./get-support.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GetSupportComponent implements OnChanges, OnInit {
  /**
   * @description
   * Support that should be hidden from view.
   */
  @Input() public hiddenSupport?: SupportProvided | SupportProvided[];

  public providedSupport: SupportProps[];

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.providedSupport = [];
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.hiddenSupport) {
      const currentValue = changes.hiddenSupport.currentValue;
      const hiddenSupport = Array.isArray(currentValue)
        ? currentValue
        : [currentValue];

      this.setupSupport(hiddenSupport);
    }
  }

  public ngOnInit(): void {
    this.setupSupport();
  }

  private setupSupport(hiddenSupport: SupportProvided[] = []): void {
    this.providedSupport = [
      {
        name: 'Provider Identity Portal',
        email: this.config.emails.providerIdentitySupport,
      },
      ...ArrayUtils.insertIf<SupportProps>(
        !hiddenSupport.includes('saEforms'),
        {
          name: 'Special Authority eForms',
          email: this.config.emails.specialAuthoritySupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        !hiddenSupport.includes('hcimAccountTransfer'),
        {
          name: 'HCIMWeb Account Transfer',
          email: this.config.emails.hcimWebSupportEmail,
        }
      ),
    ];
  }
}
