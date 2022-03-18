import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import {
  DemographicsSection,
  ProfileStatus,
} from '../models/profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
  PortalSectionProperty,
} from './portal-section.class';

export class DemographicsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'demographics';
    this.type = 'profile';
    this.heading = 'Personal Information';
    this.description = 'Personal and Contact Information';
  }

  public get hint(): string {
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(
      this.getStatusCode()
    )
      ? ''
      : '1 min to complete';
  }

  public get properties(): PortalSectionProperty[] {
    const { firstName, lastName, email, phone } = this.getSectionStatus();
    return this.getStatusCode() === StatusCode.COMPLETED
      ? [
          {
            key: 'fullName',
            value: `${firstName} ${lastName}`,
          },
          {
            key: 'email',
            value: email,
          },
          {
            key: 'phone',
            value: phone,
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
      label: 'Update',
      route: ProfileRoutes.routePath(ProfileRoutes.PERSONAL_INFO_PAGE),
      disabled:
        statusCode === StatusCode.ERROR ||
        statusCode === StatusCode.NOT_AVAILABLE,
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
      ? ''
      : statusCode === StatusCode.COMPLETED
      ? 'Completed'
      : 'Incomplete';
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): DemographicsSection {
    return this.profileStatus.status.demographics;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.demographics.statusCode;
  }
}
