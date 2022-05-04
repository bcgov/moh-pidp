import { AccessType } from '@bcgov/shared/data-access';

export interface Transaction {
  partyId: number;
  accessType: AccessType;
  requestedOn: string;
}
