import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { DocumentsRoutes } from '@app/features/documents/documents.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import {
  IPortalSection,
  PortalSectionAction,
  PortalSectionKey,
  PortalSectionType,
} from './portal-section.class';

export class TransactionsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public type: PortalSectionType;
  public heading: string;
  public description: string;

  public constructor(private router: Router) {
    this.key = 'signedAcceptedDocuments';
    this.type = 'documents';
    this.heading = 'Transactions';
    this.description = 'View Transaction';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: 'View',
      route: DocumentsRoutes.routePath(DocumentsRoutes.TRANSACTIONS_PAGE),
      disabled: false,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
