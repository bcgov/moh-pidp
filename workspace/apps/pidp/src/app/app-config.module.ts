import { InjectionToken, NgModule } from '@angular/core';

import { AppEnvironment } from '../environments/environment.model';
import { environment } from '../environments/environment';

import { AppRoutes } from './app.routes';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export interface AppConfig extends AppEnvironment {
  routes: {
    auth: string;
    denied: string;
    maintenance: string;
  };
}

// Default environment configuration is for local development
// in and out of a container, and overridden in production.
export const APP_DI_CONFIG: AppConfig = {
  ...environment,
  routes: {
    auth: AppRoutes.AUTH,
    denied: AppRoutes.DENIED,
    maintenance: AppRoutes.MAINTENANCE,
  },
};

@NgModule({})
export class AppConfigModule {}
