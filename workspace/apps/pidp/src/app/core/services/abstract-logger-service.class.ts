export enum LogLevel {
  LOG = 'log',
  INFO = 'info',
  WARN = 'warn',
  ERROR = 'error',
}

export interface LogParams {
  message: string;
  data: unknown[];
}

export abstract class AbstractLoggerService {
  /**
   * @description
   * Outputs informationInformative output of logging information.
   */
  public info(message: string, ...data: unknown[]): void {
    this.dispatch(LogLevel.INFO, { message, data });
  }

  /**
   * @description
   * Outputs a warning message.
   */
  public warn(message: string, ...data: unknown[]): void {
    this.dispatch(LogLevel.WARN, { message, data });
  }

  /**
   * @description
   * Outputs an error message.
   */
  public error(message: string, ...data: unknown[]): void {
    this.dispatch(LogLevel.ERROR, { message, data });
  }

  /**
   * @description
   * Dispatch the logging event.
   */
  protected abstract dispatch(level: LogLevel, params: LogParams): void;
}
