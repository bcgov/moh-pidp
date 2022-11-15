import { Section } from '../section.model';
import { AdministratorInfoSection } from './administrator-information-section.model';

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
  'endorsements',
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

export interface OrganizationGroup extends IOrganizationGroup {
  organizationDetails: Section;
  facilityDetails: Section;
  administratorInfo: AdministratorInfoSection;
  endorsements: Section;
}
