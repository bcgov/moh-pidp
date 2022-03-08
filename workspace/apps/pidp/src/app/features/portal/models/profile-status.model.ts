import { Party, PartyCertification } from '@bcgov/shared/data-access';

import { AlertCode } from '../enums/alert-code.enum';
import { AccessSectionStatus } from './access-status.model';
import { Section } from './section.model';

export interface DemographicsSection
  extends Pick<Party, 'firstName' | 'lastName' | 'phone' | 'email'>,
    Section {}

export interface CollegeCertificationSection
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'>,
    Section {}

export interface ProfileSectionStatus {
  demographics: DemographicsSection;
  collegeCertification: CollegeCertificationSection;
}

export interface ProfileStatus {
  alerts: AlertCode[];
  // TODO temporarily merged profile and access statuses
  status: ProfileSectionStatus & AccessSectionStatus;
}
