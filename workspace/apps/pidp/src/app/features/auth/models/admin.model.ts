import { User } from '@bcgov/shared/data-access';

export interface Admin extends User {
  idir: string;
  userId: string;
}
