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
    private router: Router,
  ) {
    this.key = 'collegeCertification';
    this.heading = 'College Licence Information';
    this.description = 'Provide your College Licence if you have one.';
  }

  public get hint(): string {
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(
      this.getStatusCode(),
    )
      ? ''
      : '1 minute to complete';
  }

  public get properties(): PortalSectionProperty[] {
    const statusCode = this.getStatusCode();
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(statusCode)
      ? [
          {
            key: 'status',
            value:
              statusCode === StatusCode.COMPLETED
                ? this.getSectionStatus().licenceDeclared
                  ? 'Verified'
                  : 'No College Licence'
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
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Update',
      route: ProfileRoutes.routePath(
        this.getSectionStatus().hasCpn
          ? ProfileRoutes.COLLEGE_LICENCE_INFO
          : ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
      ),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
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
