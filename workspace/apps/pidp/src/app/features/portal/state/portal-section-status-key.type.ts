import { AccessGroup } from './access/access-group.model';
import { ProfileGroup } from './profile/profile-group.model';
import { TrainingGroup } from './training/training-group.model';

/**
 * @description
 * Set of unique identifiers for portal group
 * sections that have a status.
 */
export type PortalSectionStatusKey =
  | keyof ProfileGroup
  | keyof AccessGroup
  | keyof TrainingGroup;
