import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faChartSimple, faFileLines, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class PHRPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  private readonly phrWebsite: string;
  public faFileLines = faFileLines;
  public faUserCheck = faUserCheck;
  public keyWords: string[];

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'phr';
    this.heading = 'PHR System';
    this.description =
      'The PHR is an online tool used by primary care providers throughout the province to indicate their ability to take on new patients.';

    this.phrWebsite = 'https://bchealthprovider.ca';
    this.keyWords =
      profileStatus.status.phr.keyWords || [];
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: 'View',
      route: AccessRoutes.routePath(AccessRoutes.PHR),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faFileLines;
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  public get status(): string {
    switch (this.getStatusCode()) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to participate in the PHR';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.phr.statusCode;
  }
}
