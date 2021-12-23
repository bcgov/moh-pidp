import { User } from './user.model';

export interface BcscUser extends User {
  id?: number;
  hpdid: string; // BCSC GUID
}
