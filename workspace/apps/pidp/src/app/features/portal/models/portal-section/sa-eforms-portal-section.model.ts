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

export class SaEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'saEforms';
    this.type = 'access';
    this.heading = 'Special Authority eForms';
    this.description = `Enrol here for access to PharmaCare's Special Authority eForms application.`;
  }

  public get hint(): string {
    return '1 min to complete';
  }

  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    const collegeCertStatusCode =
      this.profileStatus.status.collegeCertification.statusCode;
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Request',
      route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
      disabled: !(
        demographicsStatusCode === StatusCode.COMPLETED &&
        collegeCertStatusCode === StatusCode.COMPLETED
      ),
    };
  }

  public get statusType(): AlertType {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.AVAILABLE
      ? 'You are eligible to use Special Authority eForms'
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
