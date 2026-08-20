import { Party } from '@bcgov/shared/data-access';

 
export type PersonalInformation = Pick<
  Party,
  | 'preferredFirstName'
  | 'preferredMiddleName'
  | 'preferredLastName'
    | 'mailingAddress'
    | 'phone'
    | 'email'
  >;
