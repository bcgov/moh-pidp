import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AlertType } from '@bcgov/shared/ui';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faFileLines} from '@fortawesome/free-solid-svg-icons';
import { faSyringe, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { ProfileStatus } from '../../models/profile-status.model';
import { IPortalSection } from '../portal-section.model';

export class ImmsBCPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public faSyringe = faSyringe;
  public faUserCheck = faUserCheck;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'immsbc';
    this.heading = 'ImmsBC';
    this.description = `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna `;
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
      route: AccessRoutes.routePath(AccessRoutes.IMMSBC),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();

    switch (statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use Special Authority eForms';
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
    return this.profileStatus.status.immsbc.statusCode;
  }
}
