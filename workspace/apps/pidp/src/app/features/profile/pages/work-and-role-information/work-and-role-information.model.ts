import { Facility, Party } from '@bcgov/shared/data-access';

export interface WorkAndRoleInformationModel
  extends Pick<Party, 'jobTitle'>,
    Pick<Facility, 'facilityName' | 'facilityAddress'> {}
