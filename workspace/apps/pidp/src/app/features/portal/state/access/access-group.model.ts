import { Section } from '../section.model';
import { SaEformsSection } from './sa-eforms-section.model';

/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const accessSectionKeys = [
  'saEforms',
  'hcimAccountTransfer',
  'hcimEnrolment',
  'sitePrivacySecurityChecklist',
  'driverFitness',
  'uci',
  'msTeams',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type AccessSectionKey = typeof accessSectionKeys[number];

/**
 * @description
 * Typing for a group generated from a union.
 */
export type IAccessGroup = {
  [K in AccessSectionKey]: Section;
};

export interface AccessGroup extends IAccessGroup {
  saEforms: SaEformsSection;
  hcimAccountTransfer: Section;
  hcimEnrolment: Section;
  sitePrivacySecurityChecklist: Section;
  driverFitness: Section;
  uci: Section;
  msTeams: Section;
}
