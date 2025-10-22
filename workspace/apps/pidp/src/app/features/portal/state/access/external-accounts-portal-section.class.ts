import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faAddressBook, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccountsRoutes } from '@app/features/accounts/accounts.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class ExternalAccountsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faUserCheck = faUserCheck;
  public faAddressBook = faAddressBook;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'externalAccounts';
    this.heading = 'External Accounts';
    this.description =
      'Bring your own accounts to OneHealthID and link them to your profile for accessing onboarded systems.';
    this.keyWords = profileStatus.status.externalAccounts.keyWords || [];
    this.completedMessage = '';
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
      route: AccountsRoutes.routePath(AccountsRoutes.EXTERNAL_ACCOUNTS),
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
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faAddressBook;
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
    return this.profileStatus.status.externalAccounts.statusCode;
  }
}
