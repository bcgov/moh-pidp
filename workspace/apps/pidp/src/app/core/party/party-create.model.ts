import { User } from '@app/features/auth/models/user.model';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface PartyCreate
  extends Pick<User, 'userId' | 'firstName' | 'lastName'> {}
