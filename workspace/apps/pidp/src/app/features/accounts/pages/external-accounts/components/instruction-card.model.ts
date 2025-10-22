/**
 * @description
 * Properties of an instruction card.
 */
export interface InstructionCard {
  /**
   * Id of the card, used to identify the card
   */
  id: number;
  /**
   * @description
   * Icon of the card
   */
  icon: string;
  /**
   * @description
   * Instruction card title
   */
  title: string;
  /**
   * @description
   * Instruction card description
   */
  description: string;
  /**
   * @description
   * Instruction card content
   */
  type: 'dropdown' | 'input' | 'verification' | 'final';
  /**
   * @description
   * Placeholder for the input field
   */
  placeholder?: string;
  /**
   * @description
   * Options for the dropdown field
   */
  options?: Array<{ label: string; value: string }>;
  /**
   * @description
   * Display text on a link
   * that is shown in the card.
   */
  linkText?: string;
  /**
   * @description
   * Display text on action button.
   */
  buttonText?: string;
}
