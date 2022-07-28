import { PartyLicenceDeclaration } from '@bcgov/shared/data-access';

import { Section } from '../section.model';

/**
 * @description
 * College certification HTTP response model for a section.
 */
export interface CollegeCertificationSection extends Section {
  licenceDeclared: boolean;
}
