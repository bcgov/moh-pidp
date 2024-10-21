import { Observable } from 'rxjs';

import { IconProp } from '@fortawesome/fontawesome-svg-core';

import { AlertType } from '@bcgov/shared/ui';

import { AccessSectionAction } from './access-section-action.model';
import { AccessSectionProperty } from './access-section-property.model';
import { AccessSectionKey } from './access/access-group.model';

export interface IAccessSection {
  /**
   * @description
   * Unique key for the section.
   */
  readonly key: AccessSectionKey;
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
  properties?: AccessSectionProperty[];
  /**
   * @description
   * Action performed by the card.
   */
  action: AccessSectionAction;
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
  /**
   * @description
   * Icon to be displayed on the card.
   */
  icon: IconProp;
  /**
   @description
   * Hidden Keywords for the card.
   */
  keyWords?: string[];
}
