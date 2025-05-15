import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class UserAccessAgreementPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'userAccessAgreement';
    this.heading = 'User Access Agreement';
    this.description = 'Sign and accept agreement to access health systems.';
  }

  public get hint(): string {
    return '8 minutes to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: this.getStatusCode() === StatusCode.COMPLETED ? 'View' : 'Sign',
      route: ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT),
      disabled: this.getStatusCode() === StatusCode.NOT_AVAILABLE,
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
    return this.profileStatus.status.userAccessAgreement?.statusCode;
  }
}
