import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { ShellRoutes } from '@app/features/shell/shell.routes';
import { TrainingRoutes } from '@app/features/training/training.routes';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class ComplianceTrainingPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'complianceTraining';
    this.heading = 'Compliance Training Video';
    this.description = 'Description of the training provided by the video.';
  }

  public get hint(): string {
    return '15 minutes to complete';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    const demographicsStatusCode =
      this.profileStatus.status.demographics.statusCode;
    return {
      label: this.getStatusCode() === StatusCode.COMPLETED ? 'View' : 'Watch',
      route: TrainingRoutes.routePath(TrainingRoutes.COMPLIANCE_TRAINING),
      disabled: demographicsStatusCode !== StatusCode.COMPLETED,
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
    return this.profileStatus.status.complianceTraining?.statusCode;
  }
}
