import { Address } from '@bcgov/shared/data-access';

export interface PersonalInformationModel {
  id: number;
  preferredFirstName: string;
  preferredMiddleName: string;
  preferredLastName: string;
  mailingAddress: Address;
  phone: string;
  email: string;
}
