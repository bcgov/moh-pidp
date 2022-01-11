import { User } from '@bcgov/shared/data-access';

export interface BcscUser extends User {
  id?: number;
  hpdid: string; // BCSC GUID
}
