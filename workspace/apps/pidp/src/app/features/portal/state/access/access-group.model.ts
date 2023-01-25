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
  'prescription-refill-eforms',
  'hcimAccountTransfer',
  'hcimEnrolment',
  'sitePrivacySecurityChecklist',
  'driverFitness',
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
  prescriptionRefillEforms: Section;
  bcProviderApplication: Section;
  hcimAccountTransfer: Section;
  hcimEnrolment: Section;
  sitePrivacySecurityChecklist: Section;
  driverFitness: Section;
  msTeams: Section;
}
