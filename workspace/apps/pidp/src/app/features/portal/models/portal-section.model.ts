import { AlertType } from '@bcgov/shared/ui';

export interface PortalSection {
  icon: string;
  type: string;
  heading: string;
  hint?: string;
  description: string;
  properties?: { key: string; value: string | number; label?: string }[];
  actionLabel?: string;
  actionDisabled: boolean;
  // TODO drop route since they now don't all route
  route?: string;
  statusType?: AlertType;
  status?: string;
}
