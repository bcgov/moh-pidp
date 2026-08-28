import { PartyLicenceDeclaration } from '@bcgov/shared/data-access';

 
export type PartyLicenceDeclarationInformation = Pick<PartyLicenceDeclaration, 'collegeCode' | 'licenceNumber'>;
