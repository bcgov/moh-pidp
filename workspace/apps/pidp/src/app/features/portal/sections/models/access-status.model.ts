import { Section } from './section.model';

/**
 * @description
 * Access group status for each section.
 */
export interface AccessSectionStatus {
  saEforms: Section;
  hcimAccountTransfer: Section;
  hcimEnrolment: Section;
  sitePrivacySecurityChecklist: Section;
}

/**
 * @description
 * HTTP response for access group.
 *
 * NOTE:
 * Merged into a single response, which will be separated into individual endpoints
 * @see profiles-status.model.ts (ProfileStatus)
 */
// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface AccessStatus {}
