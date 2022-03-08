import { AlertType } from '@bcgov/shared/ui';

import { ProfileSectionStatus } from './profile-status.model';

export type PortalSectionKey =
  | keyof ProfileSectionStatus
  | 'hcimWebEnrolment'
  | 'signedAcceptedDocuments';

export interface PortalSection {
  key: PortalSectionKey;
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;
}

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
