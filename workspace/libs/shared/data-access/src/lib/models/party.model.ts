import { Address } from './address.model';
import { Facility } from './facility.model';
import { PartyCertification } from './party-certification.model';
import { User } from './user.model';

export interface Party extends User {
  id?: number;
  firstName: string;
  lastName: string;
  preferredFirstName: string;
  preferredMiddleName: string;
  preferredLastName: string;
  dateOfBirth: string;
  email: string;
  phone: string;
  mailingAddress: Address;
  partyCertification: PartyCertification;
  jobTitle: string;
  facility: Facility;
}
