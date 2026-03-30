import {
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import {
  EnvironmentProviders,
  InjectionToken,
  Provider,
  inject,
  provideEnvironmentInitializer,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  Routes,
  provideRouter,
  withInMemoryScrolling,
  withRouterConfig,
} from '@angular/router';

import {
  INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
  IncludeBearerTokenCondition,
  createInterceptorCondition,
  includeBearerTokenInterceptor,
} from 'keycloak-angular';
import { provideEnvironmentNgxMask } from 'ngx-mask';

import { provideHttpInterceptors } from '@bcgov/shared/data-access';
import { provideMaterialConfig, provideNgxProgressBar } from '@bcgov/shared/ui';

import { provideKeycloakAngular } from '@app/modules/keycloak/keycloak.config';

export interface CoreOptions {
  routes: Routes;
}

// create unique injection token for the guard
export const CORE_GUARD = new InjectionToken<string>('CORE_GUARD');

const bearerTokenCondition =
  createInterceptorCondition<IncludeBearerTokenCondition>({
    urlPattern: /^(.+)?$/i,
  });

export function provideCore(
  options: CoreOptions,
): (Provider | EnvironmentProviders)[] {
  return [
    provideKeycloakAngular(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    { provide: CORE_GUARD, useValue: 'CORE_GUARD' },
    {
      provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
      useValue: [bearerTokenCondition],
    },
    provideAnimations(),
    provideNgxProgressBar(),
    provideMaterialConfig(),
    provideEnvironmentNgxMask(),
    provideHttpClient(
      // DI-based interceptors must be explicitly enabled.
      withInterceptorsFromDi(),
      withInterceptors([includeBearerTokenInterceptor]),
    ),
    provideHttpInterceptors(),
    provideRouter(
      options.routes,
      withRouterConfig({
        onSameUrlNavigation: 'reload',
      }),
      // WARNING: Does not work as expected with Material SideNav
      // being the scrollable content container.
      // @see app.component.ts for implementation
      // scrollPositionRestoration: 'enabled',
      // anchorScrolling: 'enabled',
      withInMemoryScrolling(),
      // disabled debug tracing
      // withDebugTracing()
    ),
    // order matters
    // (especially when accessing some of the above defined providers)
    // init has to be last
    {
      provide: provideEnvironmentInitializer,
      multi: true,
      useValue(): void {
        const coreGuard = inject(CORE_GUARD, {
          skipSelf: true,
          optional: true,
        });

        if (coreGuard) {
          throw new TypeError(
            'provideCore() can be used only once per application (and never in library)',
          );
        }
      },
    },
  ];
}
