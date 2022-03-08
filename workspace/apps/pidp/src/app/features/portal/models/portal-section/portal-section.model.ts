import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessSectionStatus } from '../access-status.model';
import { ProfileSectionStatus } from '../profile-status.model';

/**
 * @description
 * Set of unique identifiers for portal sections.
 */
export type PortalSectionKey =
  | keyof ProfileSectionStatus
  | keyof AccessSectionStatus
  | 'signedAcceptedDocuments';

export interface PortalSectionProperty {
  key: string;
  value: string | number;
  label?: string;
}

export interface PortalSectionAction {
  label: string;
  route: string;
  disabled: boolean;
}

export interface IPortalSection {
  readonly key: PortalSectionKey;
  type: 'profile' | 'access' | 'training' | 'documents';
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;

  performAction(): Observable<void> | void;
}
