import { User } from '@bcgov/shared/data-access';

export interface BcscUser extends User {
  hpdid: string; // BCSC identifier
  userId: string; // Keycloak identifier
  birthdate: string;
}
