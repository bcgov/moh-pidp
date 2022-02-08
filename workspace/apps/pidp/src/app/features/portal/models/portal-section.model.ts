import { AlertType } from '@bcgov/shared/ui';

export enum PortalSectionType {
  PERSONAL_INFORMATION = 'personal-information',
  COLLEGE_LICENCE_INFORMATION = 'college-licence-information',
  SA_EFORMS = 'special-authority-eforms',
  SIGNED_ACCEPTED_DOCUMENTS = 'signed-accepted-documents',
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

export interface PortalSection {
  icon: string;
  type: PortalSectionType;
  heading: string;
  hint?: string;
  description: string;
  properties?: PortalSectionProperty[];
  action: PortalSectionAction;
  statusType?: AlertType;
  status?: string;
}

// TODO not enough time to put this in place
// export enum ProfileStatusType {
//   PERSONAL_INFORMATION = 'personal-information',
//   COLLEGE_LICENCE_INFORMATION = 'college-licence-information',
// }

// export enum AccessRequestType {
//   SA_EFORMS = 'special-authority-eforms',
// }

// export enum ProfileAccessType {
//   SIGNED_ACCEPTED_DOCUMENTS = 'signed-accepted-documents',
// }

// // eslint-disable-next-line @typescript-eslint/no-empty-interface
// export interface ProfilePortalSection extends PortalSection {
//   type: ProfileStatusType;
// }

// // eslint-disable-next-line @typescript-eslint/no-empty-interface
// export interface AccessPortalSection extends PortalSection {
//   type: AccessRequestType;
// }

// // eslint-disable-next-line @typescript-eslint/no-empty-interface
// export interface YourProfileSection extends PortalSection {
//   type: ProfileAccessType;
// }
