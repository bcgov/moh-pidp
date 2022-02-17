import { PartyCertification } from '@bcgov/shared/data-access';

/* eslint-disable @typescript-eslint/no-empty-interface */
export interface CollegeLicenceInformationModel
  extends Pick<PartyCertification, 'collegeCode' | 'licenceNumber'> {}
