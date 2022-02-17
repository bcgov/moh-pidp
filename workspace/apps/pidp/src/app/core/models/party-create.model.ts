import { BcscUser } from '@app/features/auth/models/bcsc-user.model';

// TODO don't like where this is located need app specific libs
// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface PartyCreate
  extends Pick<BcscUser, 'userId' | 'firstName' | 'lastName'> {}
