import { DOCUMENT } from '@angular/common';
import { InjectionToken, inject } from '@angular/core';

export const WINDOW = new InjectionToken<Window>(
  'An abstraction over global window object',
  {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    factory: (): Window => inject(DOCUMENT).defaultView!,
  }
);
