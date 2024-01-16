import { InjectionToken, inject } from '@angular/core';

import { WINDOW } from './window.provider';

export const NAVIGATOR = new InjectionToken<Navigator>(
  'An abstraction over window.navigator object',
  {
    factory: (): Navigator => inject(WINDOW).navigator,
  },
);
