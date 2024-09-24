import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { BannerFindResponse } from './banner-find.response.model';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

@Injectable({
  providedIn: 'root',
})
export class LoginResource {
  public constructor(
    private apiResource: ApiHttpClient
  ) {
  }

  public findBanners(
    component: string,
  ): Observable<BannerFindResponse[]> {
    return this.apiResource
      .get<BannerFindResponse[]>(
        `banners?component=${component}`,
      )
  }
}
