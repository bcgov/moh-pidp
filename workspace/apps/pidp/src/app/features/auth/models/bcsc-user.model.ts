import { User } from '@bcgov/shared/data-access';

export interface BcscUser extends User {
  hpdid: string; // BCSC GUID
  userId: string; // Keycloak identifier
}
