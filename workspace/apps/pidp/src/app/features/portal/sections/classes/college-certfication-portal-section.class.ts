import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import {
  CollegeCertificationSection,
  ProfileStatus,
} from '../models/profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
  PortalSectionProperty,
} from './portal-section.class';

export class CollegeCertificationPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'collegeCertification';
    this.type = 'profile';
    this.heading = 'College Licence Information';
    this.description = 'College Licence Information and Validation';
  }

  public get hint(): string {
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(
      this.getStatusCode()
    )
      ? ''
      : '1 min to complete';
  }

  public get properties(): PortalSectionProperty[] {
    const statusCode = this.getStatusCode();
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    const { collegeCode, licenceNumber } = this.getSectionStatus();
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(statusCode)
      ? [
          {
            key: 'collegeCode',
            value: collegeCode,
          },
          {
            key: 'licenceNumber',
            value: licenceNumber,
            label: 'College Licence Number:',
          },
          {
            key: 'status',
            value:
              statusCode !== StatusCode.ERROR &&
              demographicsStatusCode === StatusCode.COMPLETED
                ? 'Verified'
                : 'Not Verified',
            label: 'College Licence Status:',
          },
        ]
      : [];
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    return {
      label: 'Update',
      route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE),
      disabled:
        demographicsStatusCode !== StatusCode.COMPLETED ||
        this.getStatusCode() === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.ERROR
      ? 'danger'
      : statusCode === StatusCode.COMPLETED
      ? 'success'
      : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.ERROR
      ? 'Licence validation error'
      : statusCode === StatusCode.COMPLETED
      ? 'Completed'
      : 'Incomplete';
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): CollegeCertificationSection {
    return this.profileStatus.status.collegeCertification;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.collegeCertification.statusCode;
  }
}
