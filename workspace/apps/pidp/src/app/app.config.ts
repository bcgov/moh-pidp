import { InjectionToken } from '@angular/core';

import { environment } from '../environments/environment';
import { AppEnvironment } from '../environments/environment.model';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface AppConfig extends AppEnvironment {}

// Default environment configuration is for local development
// in and out of a container, and overridden in production.
export const APP_DI_CONFIG: AppConfig = {
  ...environment,
};
