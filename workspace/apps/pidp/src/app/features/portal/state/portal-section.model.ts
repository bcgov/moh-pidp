import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { PortalSectionAction } from './portal-section-action.model';
import { PortalSectionKey } from './portal-section-key.type';
import { PortalSectionProperty } from './portal-section-property.model';

export interface IPortalSection {
  readonly key: PortalSectionKey;
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;

  performAction(): Observable<void> | void;
}
