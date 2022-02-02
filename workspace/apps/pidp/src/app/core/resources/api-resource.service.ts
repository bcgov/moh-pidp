import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { AbstractHttpResource } from '@bcgov/shared/data-access';

import { APP_CONFIG, AppConfig } from '@app/app.config';

// TODO rename resource to ApiHttpClient
@Injectable({
  providedIn: 'root',
})
export class ApiResource extends AbstractHttpResource {
  protected uri: string;

  public constructor(@Inject(APP_CONFIG) config: AppConfig, http: HttpClient) {
    super(http);

    this.uri = config.apiEndpoint;
  }
}
