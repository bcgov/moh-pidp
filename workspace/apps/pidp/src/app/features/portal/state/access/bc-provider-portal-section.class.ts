import { NavigationExtras, Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faFileLines, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class BcProviderPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faFileLines = faFileLines;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public errorReason: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
  ) {
    this.key = 'bcProvider';
    this.heading = 'BC Provider Account';
    this.description = `A reusable credential for access to health data in BC.`;
    this.keyWords = profileStatus.status.bcProvider.keyWords || [];
    this.errorReason = profileStatus.status.bcProvider.errorReason ?? '';
  }

  public get hint(): string {
    return '';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    const route = this.getRoute();
    const navigationExtras = this.getNavigationExtras();
    return {
      label: statusCode === StatusCode.COMPLETED ? 'Edit' : 'Request',
      route: route,
      navigationExtras: navigationExtras,
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.AVAILABLE
      ? ''
      : statusCode === StatusCode.COMPLETED
        ? 'Completed'
        : 'Incomplete';
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED ? faUserCheck : faFileLines;
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.bcProvider.statusCode;
  }
  private getUserAccessAgreementStatusCode(): StatusCode {
    return this.profileStatus.status.userAccessAgreement.statusCode;
  }
  private getNavigationExtras(): NavigationExtras | undefined {
    if (this.getUserAccessAgreementStatusCode() === StatusCode.AVAILABLE) {
      return {
        queryParams: {
          'redirect-url': this.getBCProviderRoute(),
        },
      };
    }
    return undefined;
  }
  private getRoute(): string {
    if (this.getUserAccessAgreementStatusCode() === StatusCode.AVAILABLE) {
      return ProfileRoutes.routePath(ProfileRoutes.USER_ACCESS_AGREEMENT);
    }

    return this.getBCProviderRoute();
  }
  private getBCProviderRoute(): string {
    const statusCode = this.getStatusCode();
    switch (statusCode) {
      case StatusCode.COMPLETED:
        return AccessRoutes.routePath(AccessRoutes.BC_PROVIDER_EDIT);
      case StatusCode.AVAILABLE:
        return AccessRoutes.routePath(AccessRoutes.BC_PROVIDER_APPLICATION);
      default:
        return '';
    }
  }
}
