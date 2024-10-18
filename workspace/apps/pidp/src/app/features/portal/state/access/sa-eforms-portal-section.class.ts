import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faFileLines, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';
import { SaEformsSection } from './sa-eforms-section.model';

export class SaEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faFileLines = faFileLines;
  public faUserCheck = faUserCheck;
  public keyWords: string[];

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'saEforms';
    this.heading = 'Special Authority eForms';
    this.description = `Enrol here for access to PharmaCare's Special Authority eForms application.`;
    this.keyWords = profileStatus.status.saEforms.keyWords || [];
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
      route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const { statusCode, incorrectLicenceType } = this.getSectionStatus();

    switch (statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use Special Authority eForms';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return incorrectLicenceType
          ? 'Pharmacy Technicians can not apply for Special Authority eForms'
          : 'Incomplete';
    }
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faFileLines;
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): SaEformsSection {
    return this.profileStatus.status.saEforms;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.saEforms.statusCode;
  }
}
