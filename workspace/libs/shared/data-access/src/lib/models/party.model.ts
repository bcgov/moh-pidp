import { Address } from './address.model';
import { User } from './user.model';

export interface Party extends User {
  physicalAddress: Address;
}
