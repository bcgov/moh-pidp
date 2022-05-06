import { AlertCode } from '../enums/alert-code.enum';
import { AccessGroup } from '../state/access/access-group.model';
import { OrganizationGroup } from '../state/organization/organization-group.model';
import { ProfileGroup } from '../state/profile/profile-group.model';
import { TrainingGroup } from '../state/training/training-group.model';

/**
 * @description
 * HTTP response for profile status.
 *
 * NOTE
 * Merged all response into ProfileStatus, which will be
 * separated into different endpoints at an appropriate
 * time to avoid unnecessary optimization early in the
 * project.
 */
export interface ProfileStatus {
  alerts: AlertCode[];
  status: ProfileGroup & AccessGroup & OrganizationGroup & TrainingGroup;
}
