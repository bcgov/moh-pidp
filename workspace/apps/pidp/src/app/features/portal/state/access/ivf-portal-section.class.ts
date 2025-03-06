import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faSyringe, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class IvfPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public faSyringe = faSyringe;
  public faUserCheck = faUserCheck;
  public heading: string;
  public description: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'ivf';
    this.heading = 'In Vitro Fertilization (IVF)';
    this.description = `In vitro fertilization, also called IVF, is a complex series of procedures that can lead to a pregnancy.
Learn more > `;
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
      route: AccessRoutes.routePath(AccessRoutes.IVF),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();

    switch (statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use IVF';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faSyringe;
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.ivf.statusCode;
  }
}
