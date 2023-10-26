import { Section } from '../section.model';

/**
 * @description
 * Dashboard info HTTP response model. Note that this is created as a "Section" out of convenience, and does not
 * actually correspond to any cards on the Portal page.
 */
export interface DashboardInfoSection extends Section {
  displayFullName: string;
  collegeCode?: number;
}
