import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  /**
   * @description
   * Server-side or connection error message.
   */
  public getServerMessage(error: HttpErrorResponse): string {
    return !navigator.onLine
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
