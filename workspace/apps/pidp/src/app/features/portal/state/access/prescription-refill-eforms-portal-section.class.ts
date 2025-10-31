import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import {
  faPrescriptionBottleMedical,
  faUserCheck,
} from '@fortawesome/free-solid-svg-icons';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { Constants } from '@app/shared/constants';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';
import { Section } from '../section.model';

export class PrescriptionRefillEformsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faPrescriptionBottleMedical = faPrescriptionBottleMedical;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'prescriptionRefillEforms';
    this.heading = 'Provincial Prescription Renewal Support Service eForm';
    this.description = `Please be informed that the Provincial Prescription Renewal Support Service (PPRSS) will be ending on Friday, October 31 2025.`;
    this.keyWords =
      profileStatus.status.prescriptionRefillEforms.keyWords || [];
    this.completedMessage = Constants.enrolledText;
  }

  public get hint(): string {
    return '1 minute to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const statusCode = this.getStatusCode();
    return {
      label: statusCode === StatusCode.COMPLETED ? 'View' : 'Signup',
      route: AccessRoutes.routePath(AccessRoutes.PRESCRIPTION_REFILL_EFORMS),
      disabled: statusCode === StatusCode.NOT_AVAILABLE,
    };
  }

  public get statusType(): AlertType {
    return this.getStatusCode() === StatusCode.COMPLETED ? 'success' : 'warn';
  }

  public get status(): string {
    switch (this.getSectionStatus().statusCode) {
      case StatusCode.AVAILABLE:
        return 'You are eligible to use the Provincial Prescription Renewal Support Service eForm';
      case StatusCode.COMPLETED:
        return 'Completed';
      default:
        return 'Incomplete';
    }
  }

  public get icon(): IconProp {
    const statusCode = this.getStatusCode();
    return statusCode === StatusCode.COMPLETED
      ? faUserCheck
      : faPrescriptionBottleMedical;
  }

  public performAction(): Observable<void> | void {
    return;
  }

  private getSectionStatus(): Section {
    return this.profileStatus.status.prescriptionRefillEforms;
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.prescriptionRefillEforms.statusCode;
  }
}
