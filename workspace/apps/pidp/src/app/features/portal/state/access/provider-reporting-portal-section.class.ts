import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faUserCheck, faUsers } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';
import { Section } from '../section.model';

export class ProviderReportingPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faUsers = faUsers;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'providerReportingPortal';
    this.heading = 'Provider Reporting Portal';
    this.description = `Enrol here for access to the Provider Reporting Portal`;
    this.keyWords = profileStatus.status.providerReportingPortal.keyWords || [];
    this.completedMessage = this.profileStatus.status.providerReportingPortal.completedMessage;
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
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Signup',
      route: AccessRoutes.routePath(AccessRoutes.PROVIDER_REPORTING_PORTAL),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    switch (this.getSectionStatus().statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use the Provider Reporting Portal';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faUsers;
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): Section {
    return this.profileStatus.status.providerReportingPortal;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.providerReportingPortal.statusCode;
  }
}
