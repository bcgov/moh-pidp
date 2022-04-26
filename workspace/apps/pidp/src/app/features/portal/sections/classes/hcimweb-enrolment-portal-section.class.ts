import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../models/profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
  PortalSectionType,
} from './portal-section.class';

export class HcimwebEnrolmentPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: PortalSectionType;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'hcimEnrolment';
    this.type = 'access';
    this.heading = 'HCIMWeb Enrolment';
    this.description =
      'First time users enrol here for access to the HCIMWeb application';
  }

  public get hint(): string {
    return '3 min to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    return {
      label: 'Request',
      route: AccessRoutes.routePath(AccessRoutes.HCIM_ENROLMENT_PAGE),
      disabled: demographicsStatusCode !== StatusCode.COMPLETED,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? 'Completed' : 'Incomplete';
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    // TODO remove null check once API exists
    return this.profileStatus.status.hcimEnrolment?.statusCode;
  }
}
