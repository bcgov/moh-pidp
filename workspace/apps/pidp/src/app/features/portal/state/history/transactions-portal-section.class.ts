import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { HistoryRoutes } from '@app/features/history/history.routes';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { PortalSectionAction } from '../portal-section-action.model';
import { PortalSectionKey } from '../portal-section-key.type';
import { IPortalSection } from '../portal-section.model';

export class TransactionsPortalSection implements IPortalSection {
  public readonly key: PortalSectionKey;
  public heading: string;
  public description: string;

  public constructor(private readonly router: Router) {
    this.key = 'signedAcceptedDocuments';
    this.heading = 'Transactions';
    this.description = 'View Transactions';
  }

  /**
   * @description
   * Get the properties that define the action on the section.
   */
  public get action(): PortalSectionAction {
    return {
      label: 'View',
      route: HistoryRoutes.routePath(HistoryRoutes.TRANSACTIONS),
      disabled: false,
    };
  }

  public performAction(): void | Observable<void> {
    this.router.navigate([ShellRoutes.routePath(this.action.route)]);
  }
}
