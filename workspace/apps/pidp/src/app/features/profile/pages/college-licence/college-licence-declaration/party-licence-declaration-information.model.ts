import { PartyLicenceDeclaration } from '@bcgov/shared/data-access';

/* eslint-disable @typescript-eslint/no-empty-interface */
export type PartyLicenceDeclarationInformation = Pick<PartyLicenceDeclaration, 'collegeCode' | 'licenceNumber'>;
