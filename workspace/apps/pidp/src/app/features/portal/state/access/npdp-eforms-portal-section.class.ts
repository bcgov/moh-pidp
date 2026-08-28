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
import { Constants } from '@app/shared/constants';

export class NpdpEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faSyringe = faSyringe;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'npdpEforms';
    this.heading =
      'Exceptional Coverage: National PharmaCare and Mifepristone/Misoprostol';
    this.description = `Apply for exceptional coverage for PharmaCare Plan NP benefits and Mifepristone/Misoprostol (Plan Z).`;
    this.keyWords = profileStatus.status.npdpEforms.keyWords || [];
    this.completedMessage = Constants.accessGrantedText;
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
      route: AccessRoutes.routePath(AccessRoutes.NPDP_EFORMS),
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
        return 'You are eligible to use Exceptional Coverage: National PharmaCare and Mifepristone/Misoprostol';
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
    return this.profileStatus.status.npdpEforms.statusCode;
  }
}
