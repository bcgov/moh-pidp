import { StatusCode } from '../enums/status-code.enum';

/**
 * @description
 * Base HTTP response model for a section.
 */
export interface Section {
  statusCode: StatusCode;
  isComplete?: boolean;
}
