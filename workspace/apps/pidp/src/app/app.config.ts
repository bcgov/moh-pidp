import { InjectionToken } from '@angular/core';

import { environment } from '../environments/environment';
import { AppEnvironment } from '../environments/environment.model';
import { AdminRoutes } from './features/admin/admin.routes';
import { AuthRoutes } from './features/auth/auth.routes';
import { PortalRoutes } from './features/portal/portal.routes';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export interface AppConfig extends AppEnvironment {
  routes: {
    auth: string;
    portal: string;
    admin: string;
  };
}

// Default environment configuration is for local development
// in and out of a container, and overridden in production.
export const APP_DI_CONFIG: AppConfig = {
  ...environment,
  routes: {
    auth: AuthRoutes.BASE_PATH,
    portal: PortalRoutes.BASE_PATH,
    admin: AdminRoutes.BASE_PATH,
  },
  featureFlags: {
    isLayoutV2Enabled: true,
  },
};
