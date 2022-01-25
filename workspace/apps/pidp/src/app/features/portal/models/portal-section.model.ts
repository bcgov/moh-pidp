import { AlertType } from '@bcgov/shared/ui';

export interface PortalSection {
  icon: string;
  type: string;
  title: string;
  hint?: string;
  description: string;
  // TODO drop the null type when generation of properties is refined
  properties?: { key: string; value: string | number; label?: string }[] | null;
  actionLabel?: string;
  actionDisabled: boolean;
  route?: string;
  statusType?: AlertType;
  status?: string;
}
