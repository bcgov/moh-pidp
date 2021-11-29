import { InjectionToken, NgModule } from '@angular/core';

import { environment } from '../environments/environment';
import { AppEnvironment } from '../environments/environment.model';

import { AppRoutes } from './app.routes';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

// TODO build out configuration
export interface AppConfig extends AppEnvironment {
  routes: {
    denied: string;
    maintenance: string;
  };
}

// Default environment configuration is for local development
// in and out of a container, and overridden in production.
export const APP_DI_CONFIG: AppConfig = {
  ...environment,
  routes: {
    denied: AppRoutes.DENIED,
    maintenance: AppRoutes.MAINTENANCE,
  },
};

@NgModule({})
export class AppConfigModule {}
