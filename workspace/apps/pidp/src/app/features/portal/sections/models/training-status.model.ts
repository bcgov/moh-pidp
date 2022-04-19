import { Section } from './section.model';

/**
 * @description
 * Unique keys for training sections.
 */
export interface TrainingSectionStatus {
  complianceTraining: Section;
  transactions: Section;
}

/**
 * @description
 * HTTP response for training sections.
 */
// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface TrainingStatus {}
