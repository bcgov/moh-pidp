import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessSystemSectionStatus } from '../models/access-system-status.model';
import { ProfileSectionStatus } from '../models/profile-status.model';
import { TrainingSectionStatus } from '../models/training-status.model';

/**
 * @description
 * Set of unique identifiers for portal sections
 * that can have a specific status.
 */
export type PortalSectionStatusKey =
  | keyof ProfileSectionStatus
  | keyof AccessSystemSectionStatus
  | keyof TrainingSectionStatus;

/**
 * @description
 * Set of unique identifiers for all possible
 * portal sections.
 */
export type PortalSectionKey =
  | PortalSectionStatusKey
  // Status-less sections are listed to allow
  // their typed inclusion
  | 'signedAcceptedDocuments'
  | 'transactions';

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

export type PortalSectionType = 'profile' | 'access' | 'training' | 'documents';

export interface IPortalSection {
  readonly key: PortalSectionKey;
  type: PortalSectionType;
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;

  performAction(): Observable<void> | void;
}
