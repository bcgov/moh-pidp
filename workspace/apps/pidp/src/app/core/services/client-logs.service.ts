import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '../resources/api-http-client.service';

export interface ClientLog {
  message: string;
  logLevel: MicrosoftLogLevel;
  additionalInformation?: string;
}

export enum MicrosoftLogLevel {
  TRACE = 0,
  DEBUG = 1,
  INFORMATION = 2,
  WARNING = 3,
  ERROR = 4,
  CRITICAL = 5,
  NONE = 6,
}

@Injectable({
  providedIn: 'root',
})
export class ClientLogsService {
  public constructor(private apiResource: ApiHttpClient) {}

  public createClientLog(log: ClientLog): NoContent {
    return this.apiResource.post<NoContent>('client-logs', log).pipe(
      NoContentResponse,
      catchError((error: HttpErrorResponse) => {
        return throwError(() => error);
      }),
    );
  }
}
