import { Address } from './address.model';
import { User } from './user.model';

export interface BcscUser extends User {
  id?: number;

  hpdid: string; // BCSC GUID
  verifiedAddress: Address;
}
