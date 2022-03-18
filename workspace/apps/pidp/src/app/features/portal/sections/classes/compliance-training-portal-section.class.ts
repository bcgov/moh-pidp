import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';
import { TrainingRoutes } from '@app/features/training/training.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../models/profile-status.model';
import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
} from './portal-section.class';

export class ComplianceTrainingPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(
    private profileStatus: ProfileStatus,
    private router: Router
  ) {
    this.key = 'complianceTraining';
    this.type = 'training';
    this.heading = 'Compliance Training Video';
    this.description = `Description of the training provided by the video.`;
  }

  public get hint(): string {
    return '15 min to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: this.getStatusCode() === StatusCode.COMPLETED ? 'View' : 'Watch',
      route: AccessRoutes.routePath(TrainingRoutes.COMPLIANCE_TRAINING_PAGE),
      disabled: true,
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
    return this.profileStatus.status.complianceTraining.statusCode;
  }
}
