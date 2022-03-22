import { Party, PartyCertification } from '@bcgov/shared/data-access';

import { AlertCode } from '../../enums/alert-code.enum';
import { AccessSystemSectionStatus } from './access-system-status.model';
import { Section } from './section.model';
import { TrainingSectionStatus } from './training-status.model';

export interface DemographicsSection
  extends Pick<Party, 'firstName' | 'lastName' | 'phone' | 'email'>,
    Section {}

export interface CollegeCertificationSection
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'>,
    Section {}

/**
 * @description
 * Unique keys for profile sections.
 */
export interface ProfileSectionStatus {
  demographics: DemographicsSection;
  collegeCertification: CollegeCertificationSection;
  userAccessAgreement: Section;
}

/**
 * @description
 * HTTP response for profile sections.
 */
export interface ProfileStatus {
  alerts: AlertCode[];
  // Merged all sections into the response from ProfileStatus, which
  // will be separated into different endpoints at an appropriate time
  // to avoid unnecessary optimization early in the project
  status: ProfileSectionStatus &
    AccessSystemSectionStatus &
    TrainingSectionStatus;
}
