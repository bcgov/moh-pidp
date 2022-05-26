import { PartyCertification } from '@bcgov/shared/data-access';

/* eslint-disable @typescript-eslint/no-empty-interface */
export interface CollegeLicenceInformation
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'> {}
