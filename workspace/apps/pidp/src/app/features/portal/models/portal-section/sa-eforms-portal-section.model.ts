import { Router } from '@angular/router';

import { Observable, map } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRequestResource } from '@app/core/resources/access-request-resource.service';
import { PartyService } from '@app/core/services/party.service';
import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
} from './portal-section.model';

export class SaEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router,
    private partyService: PartyService,
    private accessRequestResource: AccessRequestResource
  ) {
    this.key = 'saEforms';
    this.type = 'access';
    this.heading = 'Special Authority eForms';
    this.description = `Enrol here for access to PharmaCare's Special Authority eForms application.`;
  }

  public get hint(): string {
    return '1 min to complete';
  }

  public get action(): PortalSectionAction {
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    const collegeCertStatusCode =
      this.profileStatus.status.collegeCertification.statusCode;
    return {
      label: this.getStatusCode() === StatusCode.COMPLETED ? 'View' : 'Request',
      route: AccessRoutes.routePath(AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE),
      disabled: !(
        demographicsStatusCode === StatusCode.COMPLETED &&
        collegeCertStatusCode === StatusCode.COMPLETED
      ),
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.AVAILABLE
      ? 'You are eligible to use Special Authority eForms'
      : statusCode === StatusCode.COMPLETED
      ? 'Completed'
      : 'Incomplete';
  }

  public performAction(): Observable<void> | void {
    if (this.getStatusCode() !== StatusCode.COMPLETED) {
      return this.accessRequestResource
        .saEforms(this.partyService.partyId)
        .pipe(
          map(() => {
            if (!this.action.route) {
              return;
            }
            this.navigate();
          })
        );
    } else {
      this.navigate();
    }
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.saEforms.statusCode;
  }

  private navigate(): void {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
