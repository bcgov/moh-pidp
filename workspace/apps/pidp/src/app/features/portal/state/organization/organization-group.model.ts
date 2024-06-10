import { Section } from '../section.model';

/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const organizationSectionKeys = ['endorsements'] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type OrganizationSectionKey = (typeof organizationSectionKeys)[number];

/**
 * @description
 * Typing for a group generated from a union.
 */
export type IOrganizationGroup = {
  [K in OrganizationSectionKey]: Section;
};

export interface OrganizationGroup extends IOrganizationGroup {
  endorsements: Section;
}
