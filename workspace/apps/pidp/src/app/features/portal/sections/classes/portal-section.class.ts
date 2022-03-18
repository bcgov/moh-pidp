import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { AccessSystemSectionStatus } from '../models/access-system-status.model';
import { ProfileSectionStatus } from '../models/profile-status.model';
import { TrainingSectionStatus } from '../models/training-status.model';

/**
 * @description
 * Set of unique identifiers for portal sections.
 */
export type PortalSectionKey =
  | keyof ProfileSectionStatus
  | keyof AccessSystemSectionStatus
  | keyof TrainingSectionStatus
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
