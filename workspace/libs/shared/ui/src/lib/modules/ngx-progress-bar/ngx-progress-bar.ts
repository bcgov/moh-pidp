import {
  EnvironmentProviders,
  Provider,
  importProvidersFrom,
} from '@angular/core';

import { NG_PROGRESS_CONFIG } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';

import { ProgressConfig } from './ngx-progress-bar.config';

export function provideNgxProgressBar(): (Provider | EnvironmentProviders)[] {
  return [
    importProvidersFrom(NgProgressHttpModule),
    {
      provide: NG_PROGRESS_CONFIG,
      useValue: ProgressConfig,
    },
  ];
}
