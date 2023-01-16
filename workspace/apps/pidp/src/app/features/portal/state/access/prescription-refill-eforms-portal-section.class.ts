import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';
import { Section } from '../section.model';

export class PrescriptionRefillEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'prescriptionRefillEforms';
    this.heading = 'Provincial Prescription Renewal Support Service eForm';
    this.description = `Enrol here for access to the Provincial Prescription Renewal Support Service eForm`;
  }

  public get hint(): string {
    return '1 min to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Signup',
      route: AccessRoutes.routePath(AccessRoutes.PRESCRIPTION_REFILL_EFORMS),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    switch (this.getSectionStatus().statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use the Provincial Prescription Renewal Support Service eForm';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): Section {
    return this.profileStatus.status.prescriptionRefillEforms;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.prescriptionRefillEforms.statusCode;
  }
}
