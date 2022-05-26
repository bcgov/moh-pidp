import { Party } from '@bcgov/shared/data-access';

import { Section } from '../section.model';

export interface DemographicsSection
  extends Pick<Party, 'firstName' | 'lastName' | 'phone' | 'email'>,
    Section {}
