import { Party } from '@bcgov/shared/data-access';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export type PersonalInformation = Pick<
  Party,
  | 'preferredFirstName'
  | 'preferredMiddleName'
  | 'preferredLastName'
    | 'mailingAddress'
    | 'phone'
    | 'email'
  >;
