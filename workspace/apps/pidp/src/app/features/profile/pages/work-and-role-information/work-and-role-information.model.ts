import { Address } from '@bcgov/shared/data-access';

export interface WorkAndRoleInformationModel {
  id: number;
  facilityName: string;
  facilityAddress: Address;
}
