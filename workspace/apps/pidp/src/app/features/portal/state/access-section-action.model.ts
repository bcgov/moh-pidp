import { NavigationExtras } from '@angular/router';

/**
 * @description
 * Properties of a portal section action.
 */
export interface AccessSectionAction {
  /**
   * Label of the action, such as Update,
   * View, or Request.
   */
  label: string;
  /**
   * @description
   * Route the action will navigate to
   * when invoked.
   */
  route: string;
  /**
   * @description
   * URL parameters
   */
  navigationExtras?: NavigationExtras;
  /**
   * @description
   * Whether the action is enable or disabled
   * typically based on state.
   */
  disabled: boolean;
  /**
   * @description
   * Indicate whether the action navigate to
   * is opened in new tab or same tab.
   * Default: in same tab.
   */
  openInNewTab?: boolean;
}
