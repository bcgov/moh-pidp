import {
  ENVIRONMENT_INITIALIZER,
  EnvironmentProviders,
  InjectionToken,
  Provider,
  inject,
} from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { Routes } from '@angular/router';

import { provideEnvironmentNgxMask } from 'ngx-mask';

export interface CoreOptions {
  routes: Routes;
}

// create unique injection token for the guard
export const CORE_GUARD = new InjectionToken<string>('CORE_GUARD');

export function provideCore(): (Provider | EnvironmentProviders)[] {
//options: CoreOptions,
  return [
    { provide: CORE_GUARD, useValue: 'CORE_GUARD' },
    provideAnimations(),
    provideEnvironmentNgxMask(),
    // provideHttpClient(),
    // provideRouter(
    //   options.routes,
    //   withRouterConfig({
    //     onSameUrlNavigation: 'reload',
    //   }),
    //   // WARNING: Does not work as expected with Material SideNav
    //   // being the scrollable content container.
    //   // @see app.component.ts for implementation
    //   // withInMemoryScrolling({
    //   //   scrollPositionRestoration: 'enabled',
    //   //   anchorScrolling: 'enabled',
    //   // }),
    //   // disabled debug tracing
    //   // withDebugTracing()
    // ),
    // order matters
    // (especially when accessing some of the above defined providers)
    // init has to be last
    {
      provide: ENVIRONMENT_INITIALIZER,
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
