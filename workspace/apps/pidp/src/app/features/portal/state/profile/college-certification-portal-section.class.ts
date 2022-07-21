import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { PortalSectionProperty } from '../portal-section-property.model';
import { IPortalSection } from '../portal-section.model';
import { CollegeCertificationSection } from './college-certification-section.model';

export class CollegeCertificationPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'collegeCertification';
    this.heading = 'College Licence Information';
    this.description = 'Provide your College Licence if you have one.';
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
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(statusCode)
      ? [
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
    const statusCode = this.getStatusCode();
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    return {
      label:
        statusCode !== StatusCode.ERROR &&
        demographicsStatusCode === StatusCode.COMPLETED
          ? 'View'
          : 'Update',
      route: ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO),
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
    return this.getSectionStatus().statusCode;
  }
}
