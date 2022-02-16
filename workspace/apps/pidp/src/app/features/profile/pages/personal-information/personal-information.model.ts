import { Party } from '@bcgov/shared/data-access';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface PersonalInformationModel
  extends Pick<
    Party,
    | 'preferredFirstName'
    | 'preferredMiddleName'
    | 'preferredLastName'
    | 'mailingAddress'
    | 'phone'
    | 'email'
  > {}
