import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { OrganizationInfoRoutes } from '@app/features/organization-info/organization-info.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { PortalSectionProperty } from '../portal-section-property.model';
import { IPortalSection } from '../portal-section.model';
import { AdministratorInfoSection } from './administrator-information-section.model';

export class AdministratorInfoPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'facilityDetails';
    this.heading = 'Administrator Information';
    this.description = `Provide your office administrator's contact information.`;
  }

  public get hint(): string {
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(
      this.getStatusCode()
    )
      ? ''
      : '1 min to complete';
  }

  public get properties(): PortalSectionProperty[] {
    const statusCode = this.getStatusCode();
    const { email } = this.getSectionStatus();
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(statusCode)
      ? [
          {
            key: 'email',
            value: email,
            label: 'Access Administrator Email:',
          },
        ]
      : [];
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    return {
      label: 'Update',
      route: OrganizationInfoRoutes.routePath(
        OrganizationInfoRoutes.ADMINISTRATOR_INFO
      ),
      disabled: demographicsStatusCode !== StatusCode.COMPLETED,
    };
  }

  public get statusType(): AlertType {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.ERROR
      ? 'danger'
      : statusCode === StatusCode.COMPLETED
      ? 'success'
      : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? 'Completed' : 'Incomplete';
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getSectionStatus(): AdministratorInfoSection {
    return this.profileStatus.status.administratorInfo;
  }

  private getStatusCode(): StatusCode {
    return this.getSectionStatus().statusCode;
  }
}
