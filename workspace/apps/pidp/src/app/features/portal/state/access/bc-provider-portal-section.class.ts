import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
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

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'bcProvider';
    this.heading = 'BC Provider';
    this.description = ``;
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
    return {
      label: statusCode === StatusCode.COMPLETED ? 'Edit' : 'Request',
      route: route,
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

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.bcProvider.statusCode;
  }
  private getRoute(): string {
    const statusCode = this.getStatusCode();
    switch (statusCode) {
      case StatusCode.COMPLETED:
        return AccessRoutes.routePath(
          AccessRoutes.BC_PROVIDER_APPLICATION_CHANGE_PASSWORD
        );
      case StatusCode.AVAILABLE:
        return AccessRoutes.routePath(AccessRoutes.BC_PROVIDER_APPLICATION);
      default:
        throw 'not implemented: ' + statusCode;
    }
  }
}
