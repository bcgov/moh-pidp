import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ApiResource } from '@core/resources/api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class ProfileResourceService {
  public constructor(private apiResource: ApiResource) {}

  public getProfileInformation(): Observable<any> {
    return this.apiResource.get(``).pipe();
  }
}
