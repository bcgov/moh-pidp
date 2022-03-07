import { AlertType } from '@bcgov/shared/ui';

// TODO swap for known key that now exists in the response
// TODO use keyof mapping from profile status to create
export enum PortalSectionType {
  PERSONAL_INFORMATION = 'personal-information',
  COLLEGE_LICENCE_INFORMATION = 'college-licence-information',
  SA_EFORMS = 'special-authority-eforms',
  HCIM_WEB_ENROLMENT = 'hcim-web-enrolment',
  SIGNED_ACCEPTED_DOCUMENTS = 'signed-accepted-documents',
}

// TODO rename section to card which will be isolated to portal
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

export interface PortalSection {
  icon?: string;
  type: PortalSectionType;
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;
}
