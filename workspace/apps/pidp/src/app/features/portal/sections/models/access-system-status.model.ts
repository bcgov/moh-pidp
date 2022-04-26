import { Section } from './section.model';

/**
 * @description
 * Unique keys for access system sections.
 */
export interface AccessSystemSectionStatus {
  saEforms: Section;
  // TODO update this to hcimAccountTransfer
  hcim: Section;
  // TODO what will the section status key actually be?
  hcimEnrolment: Section;
  sitePrivacySecurityChecklist: Section;
}

/**
 * @description
 * HTTP response for access system sections.
 */
export interface AccessSystemStatus {
  status: AccessSystemSectionStatus;
}
