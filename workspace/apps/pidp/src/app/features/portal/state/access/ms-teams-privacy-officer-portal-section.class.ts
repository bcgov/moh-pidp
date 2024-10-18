import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faUserCheck, faUserGroup } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class MsTeamsPrivacyOfficerPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faUserGroup = faUserGroup;
  public faUserCheck = faUserCheck;
  public keyWords: string[];

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'msTeamsPrivacyOfficer';
    this.heading = 'MS Teams for Clinical Use';
    this.description = `Privacy Officer enrol here for clinic access for MS Teams with Fraser Health.`;
    this.keyWords = profileStatus.status.msTeamsPrivacyOfficer.keyWords || [];
  }

  public get hint(): string {
    return '1 minute to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Request',
      route: AccessRoutes.routePath(AccessRoutes.MS_TEAMS_PRIVACY_OFFICER),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.AVAILABLE
      ? 'Clinic owners in the Fraser Health region may enrol for MS Teams access for their clinic'
      : statusCode === StatusCode.COMPLETED
        ? 'Completed'
        : 'Incomplete';
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faUserGroup;
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.msTeamsPrivacyOfficer.statusCode;
  }
}
