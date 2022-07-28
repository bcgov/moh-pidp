import { PartyLicenceDeclaration } from '@bcgov/shared/data-access';

/* eslint-disable @typescript-eslint/no-empty-interface */
export interface PartyLicenceDeclarationInformation
  extends Pick<PartyLicenceDeclaration, 'collegeCode' | 'licenceNumber'> {}
