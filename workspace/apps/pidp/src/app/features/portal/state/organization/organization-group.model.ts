import { Section } from '../section.model';

/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const organizationSectionKeys = [
  'organizationDetails',
  'facilityDetails',
  'administratorInfo',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type OrganizationSectionKey = typeof organizationSectionKeys[number];

/**
 * @description
 * Typing for a group generated from a union.
 */
export type IOrganizationGroup = {
  [K in OrganizationSectionKey]: Section;
};

/**
 * @description
 * Type used to ensure adding a new key to the tuple is
 * included in the group interface.
 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
type CheckGroup<T extends IOrganizationGroup = OrganizationGroup> = void;

export interface OrganizationGroup {
  organizationDetails: Section;
  facilityDetails: Section;
  administratorInfo: Section;
}
