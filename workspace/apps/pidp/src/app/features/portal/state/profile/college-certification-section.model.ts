import { Section } from '../section.model';

/**
 * @description
 * College certification HTTP response model for a section.
 */
export interface CollegeCertificationSection extends Section {
  hasCpn: boolean;
  licenceDeclared: boolean;
  isComplete: boolean;
}
