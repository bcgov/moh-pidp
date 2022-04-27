import { Party, PartyCertification } from '@bcgov/shared/data-access';

import { AlertCode } from '../../enums/alert-code.enum';
import { AccessSectionStatus } from './access-status.model';
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
 *
 * NOTE:
 * Merged all response into ProfileStatus, which will be separated
 * into different endpoints at an appropriate time to avoid unnecessary
 * optimization early in the project.
 * @see access-status.model.ts (AccessStatus)
 * @see training-status.model.ts (TrainingStatus)
 */
export interface ProfileStatus {
  alerts: AlertCode[];
  status: ProfileSectionStatus & AccessSectionStatus & TrainingSectionStatus;
}
