import { Section } from './section.model';

/**
 * @description
 * Unique keys for training sections.
 */
export interface TrainingSectionStatus {
  complianceTraining: Section;
}

/**
 * @description
 * HTTP response for training sections.
 *
 * NOTE:
 * Merged into a single response, which will be separated into individual endpoints
 * @see profiles-status.model.ts (ProfileStatus)
 */
// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface TrainingStatus {}
