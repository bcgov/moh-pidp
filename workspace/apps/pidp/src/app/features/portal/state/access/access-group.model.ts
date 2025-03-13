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
  'accountLinking',
  'prescription-refill-eforms',
  'hcimAccountTransfer',
  'immsbc',
  'immsBCEforms',
  'driverFitness',
  'msTeamsPrivacyOfficer',
  'msTeamsClinicMember',
  'provider-reporting-portal',
  'provincialAttachmentSystem',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type AccessSectionKey = (typeof accessSectionKeys)[number];

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
  accountLinking: Section;
  bcProvider: Section;
  hcimAccountTransfer: Section;
  immsbc: Section;
  immsBCEforms: Section;
  driverFitness: Section;
  msTeamsPrivacyOfficer: Section;
  msTeamsClinicMember: Section;
  providerReportingPortal: Section;
  provincialAttachmentSystem: Section;
}
