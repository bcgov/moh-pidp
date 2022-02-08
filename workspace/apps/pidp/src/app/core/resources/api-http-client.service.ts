import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { AbstractHttpClient } from '@bcgov/shared/data-access';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class ApiHttpClient extends AbstractHttpClient {
  protected uri: string;

  public constructor(@Inject(APP_CONFIG) config: AppConfig, http: HttpClient) {
    super(http);

    this.uri = config.apiEndpoint;
  }
}
