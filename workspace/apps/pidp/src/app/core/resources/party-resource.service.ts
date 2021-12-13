import { Injectable } from '@angular/core';

import { catchError, map, Observable } from 'rxjs';

import {
  ApiHttpErrorResponse,
  ApiHttpResponse,
  Party,
} from '@bcgov/shared/data-access';

import { ApiResourceUtilsService } from './api-resource-utils.service';
import { ApiResource } from './api-resource.service';

@Injectable({
  providedIn: 'root',
})
export class PartyResourceService {
  public constructor(
    private apiResource: ApiResource,
    private apiResourceUtilsService: ApiResourceUtilsService
  ) {}
  public getParties(): Observable<ApiHttpResponse<Party[]>> {
    return this.apiResource.get<Party[]>('parties').pipe(
      map((response: ApiHttpResponse<Party[]>) => response),
      catchError((error: ApiHttpErrorResponse) => {
        throw error;
      })
    );
  }

  public createParty(party: Party): Observable<ApiHttpResponse<Party>> {
    return this.apiResource.post<Party>('parties', party).pipe(
      map((response: ApiHttpResponse<Party>) => response),
      catchError((error: ApiHttpErrorResponse) => {
        throw error;
      })
    );
  }
}
