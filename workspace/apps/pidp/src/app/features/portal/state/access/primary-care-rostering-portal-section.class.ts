import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faChartSimple, faUserCheck } from '@fortawesome/free-solid-svg-icons';

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
  public faChartSimple = faChartSimple;
  public faUserCheck = faUserCheck;

  public constructor(private profileStatus: ProfileStatus) {
    this.key = 'primaryCareRostering';
    this.heading = 'Provincial Attachment System';
    this.description =
      'The Provincial Attachment System (PAS) is an online tool used by primary care providers throughout the province to indicate their ability to take on new patients. Through PAS, Attachment Coordinators help match patients to family physicians and nurse practitioners in their communities.';

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

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faChartSimple;
  }

  public performAction(): void | Observable<void> {
    window.open(this.action.route, '_blank');
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.primaryCareRostering.statusCode;
  }
}
