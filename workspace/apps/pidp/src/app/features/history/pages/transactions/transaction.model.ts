import { AccessTypeCode } from '@bcgov/shared/data-access';

export interface Transaction {
  partyId: number;
  accessTypeCode: AccessTypeCode;
  requestedOn: string;
}
