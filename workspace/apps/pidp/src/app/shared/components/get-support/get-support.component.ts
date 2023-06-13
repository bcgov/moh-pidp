import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnInit,
} from '@angular/core';

import { ArrayUtils } from '@bcgov/shared/utils';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { PermissionsService } from '@app/modules/permissions/permissions.service';
import { Role } from '@app/shared/enums/roles.enum';

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
export class GetSupportComponent implements OnInit {
  public providedSupport: SupportProps[];

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private permissionsService: PermissionsService
  ) {
    this.providedSupport = [];
  }

  public ngOnInit(): void {
    this.setupSupport();
  }

  // TODO start having these be registered from the modules to a service to
  //      reduce the spread of maintenance and updates, and remove the dependency
  //      on the config and permissions service from the component
  private setupSupport(): void {
    this.providedSupport = [
      {
        name: 'Provider Identity Portal',
        email: this.config.emails.providerIdentitySupport,
      },
      {
        name: 'Special Authority eForms',
        email: this.config.emails.specialAuthorityEformsSupport,
      },
      {
        name: 'Prescription Renewal Support eForm',
        email: this.config.emails.prescriptionRefillRequestEformsSupport,
      },
      {
        name: 'HCIMWeb Account Transfer',
        email: this.config.emails.hcimAccountTransferSupport,
      },
      ...ArrayUtils.insertIf<SupportProps>(
        this.permissionsService.hasRole(Role.FEATURE_PIDP_DEMO),
        {
          name: 'HCIMWeb Enrolment',
          email: this.config.emails.hcimEnrolmentSupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        this.permissionsService.hasRole(Role.FEATURE_PIDP_DEMO),
        {
          name: 'Driver Medical Fitness',
          email: this.config.emails.driverFitnessSupport,
        }
      ),
      ...ArrayUtils.insertIf<SupportProps>(
        this.permissionsService.hasRole(Role.FEATURE_PIDP_DEMO),
        {
          name: 'MS Teams for Clinical Use',
          email: this.config.emails.msTeamsSupport,
        }
      ),
    ];
  }
}
