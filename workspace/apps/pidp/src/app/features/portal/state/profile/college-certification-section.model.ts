import { PartyCertification } from '@bcgov/shared/data-access';

import { Section } from '../section.model';

export interface CollegeCertificationSection
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'>,
    Section {}
