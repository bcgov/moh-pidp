import { Section } from '../section.model';

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

/**
 * @description
 * Type used to ensure adding a new key to the tuple is
 * included in the group interface.
 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
type CheckGroup<T extends IAccessGroup = AccessGroup> = void;

export interface AccessGroup {
  saEforms: Section;
  hcimAccountTransfer: Section;
  hcimEnrolment: Section;
  sitePrivacySecurityChecklist: Section;
}
