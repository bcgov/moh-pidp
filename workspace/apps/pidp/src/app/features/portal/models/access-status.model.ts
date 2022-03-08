import { Section } from './section.model';

export interface AccessSectionStatus {
  saEforms: Section;
  hcimWebEnrolment: Section;
}

export interface AccessStatus {
  status: AccessSectionStatus;
}
