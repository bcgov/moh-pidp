import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faChartSimple, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';
import { Constants } from '@app/shared/constants';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class RsvEligibilityChecklistPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public faChartSimple = faChartSimple;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'rsvEligibilityChecklist';
    this.heading = 'RSV Eligibility Checklist eForm';
    this.description =
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed doeiusmod tempor incididunt ut labore et dolore magna aliqua.';
    this.keyWords =
      profileStatus.status.rsvEligibilityChecklist.keyWords || [];
    this.completedMessage = Constants.accessGrantedText;
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: 'View',
      route: AccessRoutes.routePath(AccessRoutes.RSV_ELIGIBILITY_CHECKLIST),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faChartSimple;
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  public get status(): string {
    switch (this.getStatusCode()) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use the RSV Eligibility Checklist eForm';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.rsvEligibilityChecklist.statusCode;
  }
}
