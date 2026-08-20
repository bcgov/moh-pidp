import {
  EnvironmentProviders,
  Provider,
} from '@angular/core';

import { provideNgProgressOptions } from 'ngx-progressbar';
import { provideNgProgressHttp } from 'ngx-progressbar/http';

import { ProgressConfig } from './ngx-progress-bar.config';

export function provideNgxProgressBar(): (Provider | EnvironmentProviders)[] {
  return [
    provideNgProgressHttp({}),
    provideNgProgressOptions(ProgressConfig),
  ];
}
