import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faLink, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class AccountLinkingPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faUserCheck = faUserCheck;
  public faLink = faLink;
  public keyWords: string[];

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'accountLinking';
    this.heading = 'Account Linking';
    this.description = 'Link different credentials together in OneHealthID';
    this.keyWords = profileStatus.status.accountLinking.keyWords || [];
  }

  public get hint(): string {
    return [StatusCode.ERROR, StatusCode.COMPLETED].includes(
      this.getStatusCode(),
    )
      ? ''
      : '1 minute to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Update',
      route: ProfileRoutes.routePath(ProfileRoutes.ACCOUNT_LINKING),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    const statusCode = this.getStatusCode();
    const statCompleted: AlertType =
      statusCode === StatusCode.COMPLETED ? 'success' : 'warn';
    return statusCode === StatusCode.ERROR ? 'danger' : statCompleted;
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faLink;
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    const statCompleted: string =
      statusCode === StatusCode.COMPLETED ? 'Completed' : 'Incomplete';
    return statusCode === StatusCode.ERROR ? '' : statCompleted;
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.accountLinking.statusCode;
  }
}
