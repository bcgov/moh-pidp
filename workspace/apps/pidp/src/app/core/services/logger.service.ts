import { Inject, Injectable } from '@angular/core';

import {
  AbstractLoggerService,
  LogLevel,
  LogParams,
} from '@bcgov/shared/utils';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { EnvironmentName } from '../../../environments/environment.model';

@Injectable({
  providedIn: 'root',
})
export class LoggerService extends AbstractLoggerService {
  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    super();
  }

  /**
   * @description
   * Pretty print JSON.
   */
  public pretty(message: string, ...data: unknown[]): void {
    this.dispatch(LogLevel.LOG, {
      message,
      data: [JSON.stringify(data, null, '\t')],
    });
  }

  /**
   * @description
   * Dispatch the logging event.
   */
  protected dispatch(level: LogLevel, params: LogParams): void {
    if (!this.shouldLog(level)) {
      return;
    }

    const message = this.colorize(level, params.message);

    if (message && Array.isArray(params.data) && params.data.length) {
      console[level](...message, ...params.data);
    } else if (message) {
      console[level](...message);
    } else {
      console.error('Logger parameters are invalid: ', params);
    }
  }

  /**
   * @description
   * Gates logging based on environment and log level.
   */
  private shouldLog(level: LogLevel): boolean {
    return (
      this.config.environmentName !== EnvironmentName.PRODUCTION ||
      level === LogLevel.ERROR ||
      level === LogLevel.WARN
    );
  }

  /**
   * @description
   * Apply colour to the console message, otherwise the use
   * the default.
   */
  private colorize(level: LogLevel, message: string): string[] {
    let color = '';

    switch (level) {
      case LogLevel.LOG:
        color = 'Yellow';
        break;
      case LogLevel.INFO:
        color = 'DodgerBlue';
        break;
      case LogLevel.ERROR:
        color = 'Red';
        break;
      case LogLevel.WARN:
        color = 'Orange';
        break;
    }

    if (color) {
      color = `color:${color}`;
    }

    return [`%c${message}`, color];
  }
}
