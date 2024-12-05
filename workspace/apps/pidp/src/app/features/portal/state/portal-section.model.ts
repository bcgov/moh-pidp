import { Observable } from 'rxjs';

import { AlertType } from '@bcgov/shared/ui';

import { PortalSectionAction } from './portal-section-action.model';
import { PortalSectionKey } from './portal-section-key.type';
import { PortalSectionProperty } from './portal-section-property.model';

/**
 * @description
 * Properties of a portal section that relate
 * to its rendering within the view.
 */
export interface IPortalSection {
  /**
   * @description
   * Unique key for the section.
   */
  readonly key: PortalSectionKey;
  /**
   * @description
   * Main heading for the section.
   */
  heading: string;
  /**
   * @description
   * Hint provided to
   */
  hint?: string;
  /**
   * @description
   * Plain text paragraph summary of the card.
   */
  description: string;
  /**
   * @description
   * List of properites that replace the
   * summary when provided.
   */
  properties?: PortalSectionProperty[];
  /**
   * @description
   * Action performed by the card.
   */
  action: PortalSectionAction;
  /**
   * @description
   * Status that relates to the styling of
   * the section within the view.
   */
  statusType?: AlertType;
  /**
   * @description
   * Plain text short summary of the status.
   */
  status?: string;
  /**
   * @description
   * Action performed when invoked.
   */
  performAction(): Observable<void> | void;

  completedMessage?: string;
  /**
   * @description
   *To Provide Completed Messagee.
   */
}
