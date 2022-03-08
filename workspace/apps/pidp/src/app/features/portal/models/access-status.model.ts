import { Section } from './section.model';

export interface AccessSectionStatus {
  saEforms: Section;
  hcim: Section;
}

export interface AccessStatus {
  status: AccessSectionStatus;
}
