import { Party, PartyCertification } from '@bcgov/shared/data-access';

export enum StatusCode {
  AVAILABLE = 1,
  COMPLETED,
  NOT_AVAILABLE,
  ERROR,
}

export enum AlertCode {
  TRANSIENT_ERROR = 1,
  PLR_BAD_STANDING,
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

export interface AccessStatus {
  saEforms: Section;
}

export interface ProfileStatus {
  alerts: AlertCode[];
  status: ProfileSectionStatus;
}

export interface ProfileStatusAlert {
  heading: string;
  content: string;
}
