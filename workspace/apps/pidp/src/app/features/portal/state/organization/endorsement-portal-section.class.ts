import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { OrganizationInfoRoutes } from '@app/features/organization-info/organization-info.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class EndorsementPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  private isRegulated: boolean;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'endorsement';
    this.heading = 'Endorsement';
    // TODO: soon this will no longer be true because OBOs will be selecting "No College Licence" rather than not filling out the card.
    this.isRegulated =
      this.profileStatus.status.collegeCertification.statusCode ===
      StatusCode.COMPLETED;
    this.description = this.isRegulated
      ? 'View and make changes to you care team'
      : 'Request endorsement from the licenced practitioners you work with to gain access to systems.';
  }

  public get hint(): string {
    return '1 min to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: this.isRegulated ? 'View' : 'Request',
      route: OrganizationInfoRoutes.routePath(
        this.isRegulated
          ? OrganizationInfoRoutes.ENDORSEMENT_REQUESTS_RECEIVED
          : OrganizationInfoRoutes.ENDORSEMENT_REQUEST
      ),
      disabled: !(
        this.profileStatus.status.demographics.statusCode ===
        StatusCode.COMPLETED
      ),
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? 'Completed' : 'Incomplete';
  }

  public performAction(): Observable<void> | void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    // TODO remove null check once API exists
    return this.profileStatus.status.endorsement?.statusCode;
  }
}
