import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import {
  EnvironmentProviders,
  InjectionToken,
  Provider,
  inject,
  provideAppInitializer,
} from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  Routes,
  provideRouter,
  withInMemoryScrolling,
  withRouterConfig,
} from '@angular/router';

import { provideEnvironmentNgxMask } from 'ngx-mask';

import { provideHttpInterceptors } from '@bcgov/shared/data-access';
import { provideMaterialConfig, provideNgxProgressBar } from '@bcgov/shared/ui';

import { provideKeycloak } from '@app/modules/keycloak/keycloak';

export interface CoreOptions {
  routes: Routes;
}

// create unique injection token for the guard
export const CORE_GUARD = new InjectionToken<string>('CORE_GUARD');

export function provideCore(
  options: CoreOptions,
): (Provider | EnvironmentProviders)[] {
  return [
    { provide: CORE_GUARD, useValue: 'CORE_GUARD' },
    provideAnimations(),
    provideNgxProgressBar(),
    provideMaterialConfig(),
    provideEnvironmentNgxMask(),
    provideHttpClient(
      // DI-based interceptors must be explicitly enabled.
      withInterceptorsFromDi(),
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
    provideKeycloak(),
    // order matters
    // (especially when accessing some of the above defined providers)
    // init has to be last
    provideAppInitializer(
      () =>
        new Promise<void>((resolve) => {
          const coreGuard = inject(CORE_GUARD, {
            skipSelf: true,
            optional: true,
          });

          if (coreGuard) {
            throw new TypeError(
              'provideCore() can be used only once per application (and never in library)',
            );
          }
          setTimeout(() => {
            console.log('Initialization complete.');
            resolve();
          }, 1000);
        }),
    ),
  ];
}
