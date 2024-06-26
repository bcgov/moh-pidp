export interface AccessSectionProperty {
  /**
   * @description
   * Human-readable label for the property.
   */
  label?: string;
  /**
   * @description
   * Key used to determine property value
   * transformations when rendered.
   */
  key: string;
  /**
   * @description
   * Raw value to be displayed when rendered.
   */
  value: string | number;
}
