import { InjectionToken, inject } from '@angular/core';

import { WINDOW } from './window.provider';

export const NAVIGATOR = new InjectionToken<Navigator>(
  'An abstraction over window.navigator object',
  {
    factory: () => inject(WINDOW).navigator,
  }
);
