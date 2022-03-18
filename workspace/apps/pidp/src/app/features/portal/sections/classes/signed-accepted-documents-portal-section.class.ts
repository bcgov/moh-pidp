import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { ShellRoutes } from '@app/features/shell/shell.routes';
import { YourDocumentsRoutes } from '@app/features/your-documents/your-documents.routes';

import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
} from './portal-section.class';

export class SignedAcceptedDocumentsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: 'profile' | 'access' | 'training' | 'documents';
  public heading: string;
  public description: string;

  public constructor(private router: Router) {
    this.key = 'signedAcceptedDocuments';
    this.type = 'documents';
    this.heading = 'View Signed or Accepted Documents';
    this.description = 'View Agreement(s)';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: 'View',
      route: YourDocumentsRoutes.routePath(
        YourDocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
      ),
      disabled: false,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
