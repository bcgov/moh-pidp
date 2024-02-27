import { Section } from '../section.model';
import { CollegeCertificationSection } from './college-certification-section.model';
import { DashboardInfoSection } from './dashboard-info-section.model';

/**
 * @description
 * Section keys as a readonly tuple to allow iteration
 * over keys at runtime to allow filtering or grouping
 * sections.
 */
export const profileSectionKeys = [
  'demographics',
  'collegeCertification',
  'userAccessAgreement',
  'accountLinking',
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type ProfileSectionKey = (typeof profileSectionKeys)[number];

/**
 * @description
 * Typing for a group generated from a union.
 */
export type IProfileGroup = {
  [K in ProfileSectionKey]: Section;
};

export interface ProfileGroup extends IProfileGroup {
  dashboardInfo: DashboardInfoSection;
  demographics: Section;
  collegeCertification: CollegeCertificationSection;
  userAccessAgreement: Section;
  accountLinking: Section;
}
