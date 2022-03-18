import { Section } from './section.model';

/**
 * @description
 * Unique keys for access system sections.
 */
export interface AccessSystemSectionStatus {
  saEforms: Section;
  hcim: Section;
}

/**
 * @description
 * HTTP response for access system sections.
 */
export interface AccessSystemStatus {
  status: AccessSystemSectionStatus;
}
