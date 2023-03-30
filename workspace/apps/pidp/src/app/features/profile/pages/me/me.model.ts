import { Party, PartyLicenceDeclaration } from '@bcgov/shared/data-access';

export interface MeInformation
  extends Pick<
    Party,
    | 'email'
    | 'phone'
    | 'preferredFirstName'
    | 'preferredMiddleName'
    | 'preferredLastName'
  > {
  licence: PartyLicenceDeclaration;
}
