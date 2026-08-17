import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { NAVIGATOR } from '@bcgov/shared/utils';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  private navigator = inject<Navigator>(NAVIGATOR);

  /**
   * @description
   * Server-side or connection error message.
   */
  public getServerMessage(error: HttpErrorResponse): string {
    return !this.navigator.onLine
      ? 'No Internet Connection'
      : `${error.status} - ${error.message}`;
  }

  /**
   * @description
   * Client-side error message.
   */
  public getClientMessage(error: Error): string {
    return error.message ? error.message : error.toString();
  }
}
