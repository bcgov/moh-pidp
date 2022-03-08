import { Party, PartyCertification } from '@bcgov/shared/data-access';

import { AlertCode } from '../enums/alert-code.enum';
import { StatusCode } from '../enums/status-code.enum';

export interface ProfileStatus {
  alerts: AlertCode[];
  status: ProfileSectionStatus;
}

export interface AccessStatus {
  saEforms: Section;
}

export interface Section {
  statusCode: StatusCode;
}

export interface PersonalInformationSection
  extends Pick<Party, 'firstName' | 'lastName' | 'phone' | 'email'>,
    Section {}

export interface CollegeCertificationSection
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'>,
    Section {}

export interface ProfileSectionStatus extends AccessStatus {
  demographics: PersonalInformationSection;
  collegeCertification: CollegeCertificationSection;
}
