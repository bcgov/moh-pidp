import { Section } from '../section.model';
import { CollegeCertificationSection } from './college-certification-section.model';
import { DemographicsSection } from './demographic-section.model';

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
] as const;

/**
 * @description
 * Union of keys generated from the tuple.
 */
export type ProfileSectionKey = typeof profileSectionKeys[number];

/**
 * @description
 * Typing for a group generated from a union.
 */
export type IProfileGroup = {
  [K in ProfileSectionKey]: Section;
};

/**
 * @description
 * Type used to ensure adding a new key to the tuple is
 * included in the group interface.
 */
// eslint-disable-next-line @typescript-eslint/no-unused-vars
type CheckGroup<T extends IProfileGroup = ProfileGroup> = void;

export interface ProfileGroup {
  demographics: DemographicsSection;
  collegeCertification: CollegeCertificationSection;
  userAccessAgreement: Section;
}
