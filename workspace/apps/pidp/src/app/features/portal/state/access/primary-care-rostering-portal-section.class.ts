import { Observable } from 'rxjs';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class PrimaryCareRosteringPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  private readonly primaryCareRosteringWebsite: string;

  public constructor(
    private profileStatus: ProfileStatus
  ) {
    this.key = 'primaryCareRostering';
    this.heading = 'Rostering B.C. Healthcare';
    this.description = `Patient rostering in family practice is a process by which patients regiser with a family practice, family physician, or team.`;
    this.primaryCareRosteringWebsite = 'https://news.gov.bc.ca/releases/2022HLTH0212-001619';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: 'View',
      route: this.primaryCareRosteringWebsite,
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
      openInNewTab: true,
    };
  }

  public performAction(): void | Observable<void> {
    window.open(this.action.route, '_blank');
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.primaryCareRostering.statusCode;
  }
}
