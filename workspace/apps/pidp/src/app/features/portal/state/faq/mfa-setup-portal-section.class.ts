import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { FaqRoutes } from '@app/features/faq/faq.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class MfaSetupPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(private readonly router: Router) {
    this.key = 'mfaSetup';
    this.heading = 'How do I setup my Multi-factor authentication?';
    this.description =
      'View instructions to setup Multi-factor authentication.';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: 'View',
      route: FaqRoutes.routePath(FaqRoutes.MFA_SETUP),
      disabled: false,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
