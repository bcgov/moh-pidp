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
import { AccessGroup } from '@app/features/portal/state/access/access-group.model';
import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

export type SupportProvided = keyof AccessGroup;

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

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private permissionsService: PermissionsService
  ) {
    this.providedSupport = [];
    this.hiddenSupport = [];
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

  // TODO start having these be registered from the modules to reduce
  //      the spread of maintenance and updates, and remove the dependency
  //      on the config and permissions service for the component
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
          email: this.config.emails.specialAuthorityEformsSupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        !hiddenSupport.includes('hcimAccountTransfer'),
        {
          name: 'HCIMWeb Account Transfer',
          email: this.config.emails.hcimAccountTransferSupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        !hiddenSupport.includes('hcimEnrolment'),
        {
          name: 'HCIMWeb Enrolment',
          email: this.config.emails.hcimEnrolmentSupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        !hiddenSupport.includes('driverFitness') &&
          this.permissionsService.hasRole(Role.FEATURE_PIDP_DEMO),
        {
          name: 'Driver Medical Fitness',
          email: this.config.emails.driverFitnessSupport,
        }
      ),
    ];
  }
}
