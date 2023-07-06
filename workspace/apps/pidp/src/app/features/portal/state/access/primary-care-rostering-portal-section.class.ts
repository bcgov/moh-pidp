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

  public constructor(private profileStatus: ProfileStatus) {
    this.key = 'primaryCareRostering';
    this.heading = 'Provincial Attachment System';
    this.description = `Provincial Attachment System in family practice is a process by which patients register with a family practice, family physician, or team.`;
    this.primaryCareRosteringWebsite = 'https://bchealthprovider.ca';
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
