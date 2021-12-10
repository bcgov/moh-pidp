import { InjectionToken, Provider } from '@angular/core';

import { RootRoutes } from './root.routes';

export const ROOT_ROUTE_CONFIG = new InjectionToken<RootRouteConfig>(
  'root-route.config'
);

export interface RootRouteConfig {
  routes: {
    denied: string;
    maintenance: string;
    pageNotFound: string;
  };
}

export const ROOT_ROUTE_DI_CONFIG: RootRouteConfig = {
  routes: {
    denied: RootRoutes.DENIED,
    maintenance: RootRoutes.MAINTENANCE,
    pageNotFound: RootRoutes.PAGE_NOT_FOUND,
  },
};

export const rootRouteConfigProvider: Provider = {
  provide: ROOT_ROUTE_CONFIG,
  useValue: ROOT_ROUTE_DI_CONFIG,
};
