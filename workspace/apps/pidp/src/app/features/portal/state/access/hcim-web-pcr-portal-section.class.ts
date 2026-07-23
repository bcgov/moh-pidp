import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faIdCard, faUserCheck } from '@fortawesome/free-solid-svg-icons';

import { AccessRoutes } from '@app/features/access/access.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';
import { Constants } from '@app/shared/constants';

import { StatusCode } from '../../enums/status-code.enum';
import { ProfileStatus } from '../../models/profile-status.model';
import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class HcimWebPcrPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;
  public faIdCard = faIdCard;
  public faUserCheck = faUserCheck;
  public keyWords: string[];
  public completedMessage: string;

  public constructor(
    private readonly profileStatus: ProfileStatus,
    private readonly router: Router,
  ) {
    this.key = 'hcimWebPcr';
    this.heading = 'Provincial Client Registry';
    this.description =
      'Repository of healthcare client demographic information, such as the Personal Health Numbers (PHN), name, date of birth, and contact details.';
    this.keyWords = profileStatus.status.hcimWebPcr?.keyWords || [];
    this.completedMessage = Constants.accessGrantedText;
  }

  public get action(): PortalSectionAction {
    return {
      label: 'View',
      route: AccessRoutes.routePath(AccessRoutes.HCIM_WEB_PCR),
      disabled: false,
    };
  }

  public get icon(): IconProp {
    return this.getStatusCode() === StatusCode.COMPLETED
      ? this.faUserCheck
      : this.faIdCard;
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }

  public get status(): string {
    return this.getStatusCode() === StatusCode.COMPLETED
      ? 'Completed'
      : 'Incomplete';
  }

  private getStatusCode(): StatusCode {
    return this.profileStatus.status.hcimWebPcr?.statusCode ?? StatusCode.AVAILABLE;
  }
}
