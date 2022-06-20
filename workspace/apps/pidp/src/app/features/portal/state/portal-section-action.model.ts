/**
 * @description
 * Properties of a portal section action.
 */
export interface PortalSectionAction {
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
   * Whether the action is enable or disabled
   * typically based on state.
   */
  disabled: boolean;
}
