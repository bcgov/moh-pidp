import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class ProvincialAttachmentSystemPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'provincialAttachmentSystem';
    this.heading = 'Provincial Attachment System';
    this.description = `The Provincial Attachment System (PAS) is an online tool used by primary care
                        providers throughout the province to indicate their ability to take on new patients.
                        Through PAS, Attachment Coordinators help match patients to family physicians and nurse
                        practitioners in their communities.`;
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: 'View',
      route: AccessRoutes.routePath(AccessRoutes.PROVINCIAL_ATTACHMENT_SYSTEM),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.provincialAttachmentSystem.statusCode;
  }
}
