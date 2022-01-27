import { AlertType } from '@bcgov/shared/ui';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  hint?: string;
  description: string;
  properties?: { key: string; value: string | number; label?: string }[];
  actionLabel?: string;
  actionDisabled: boolean;
  route?: string;
  statusType?: AlertType;
  status?: string;
}
