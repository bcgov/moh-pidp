import { Facility, Party } from '@bcgov/shared/data-access';

export interface WorkAndRoleInformation
  extends Pick<Party, 'jobTitle'>,
    Pick<Facility, 'facilityName' | 'facilityAddress'> {}
