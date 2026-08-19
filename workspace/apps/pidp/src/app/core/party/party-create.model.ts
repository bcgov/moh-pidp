import { User } from '@app/features/auth/models/user.model';

export type PartyCreate = Pick<User, 'userId' | 'firstName' | 'lastName'>;
