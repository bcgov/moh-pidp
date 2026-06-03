import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { AbstractHttpClient } from '@bcgov/shared/data-access';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class ApiHttpClient extends AbstractHttpClient {
  protected uri: string;

  public constructor() {
    const config = inject<AppConfig>(APP_CONFIG);
    const http = inject(HttpClient);

    super(http);

    this.uri = config.apiEndpoint;
  }
}
