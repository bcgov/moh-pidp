import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { DocumentsRoutes } from '@app/features/documents/documents.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
} from './portal-section.class';

export class SignedAcceptedDocumentsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(private router: Router) {
    this.key = 'signedAcceptedDocuments';
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
      route: DocumentsRoutes.routePath(
        DocumentsRoutes.SIGNED_ACCEPTED_DOCUMENTS_PAGE
      ),
      disabled: false,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
