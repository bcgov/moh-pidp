import { Router } from '@angular/router';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
} from './portal-section.model';

export class HcimWebEnrolmentPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'hcimWebEnrolment';
    this.type = 'access';
    this.heading = 'HCIM Web Enrolment';
    this.description = 'Enrol here for access to HCIM.';
  }

  public get hint(): string {
    return '3 min to complete';
  }

  public get action(): PortalSectionAction {
    return {
      label: 'Request',
      route: AccessRoutes.routePath(AccessRoutes.HCIM_WEB_ENROLMENT),
      disabled: false,
    };
  }

  public get statusType(): AlertType {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.AVAILABLE
      ? 'You are eligible to use HCIM Web Enrolment'
      : statusCode === StatusCode.COMPLETED
      ? 'Completed'
      : 'Incomplete';
  }

  public performAction(): void {
    // TODO perform access request
    // this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.collegeCertification.statusCode;
  }
}
