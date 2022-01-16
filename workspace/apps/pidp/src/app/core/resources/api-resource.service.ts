import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { AbstractResource } from '@bcgov/shared/data-access';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class ApiResource extends AbstractResource {
  public url: string;

  public constructor(@Inject(APP_CONFIG) config: AppConfig, http: HttpClient) {
    super(http);

    this.url = config.apiEndpoint;
  }
}
