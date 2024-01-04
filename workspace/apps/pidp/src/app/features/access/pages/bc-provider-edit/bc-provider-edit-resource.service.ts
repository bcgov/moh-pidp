import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { BcProviderEditInitialStateModel } from './bc-provider-edit.page';

export interface BcProviderChangePasswordRequest {
  partyId: number;
  newPassword: string;
}

@Injectable({ providedIn: 'root' })
export class BcProviderEditResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public get(partyId: number): Observable<BcProviderEditInitialStateModel> {
    const url = `parties/${partyId}/credentials/bc-provider`;
    return this.apiResource.get<BcProviderEditInitialStateModel>(url);
  }

  public changePassword(data: BcProviderChangePasswordRequest): NoContent {
    const url = `parties/${data.partyId}/credentials/bc-provider/password`;
    return this.apiResource.post<NoContent>(url, data).pipe(
      NoContentResponse,
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      }),
    );
  }
}
