import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

import {
  Observable,
  OperatorFunction,
  UnaryFunction,
  pipe,
  throwError,
} from 'rxjs';
import { catchError, map } from 'rxjs/operators';

/**
 * @description
 * Type for indicating a NoContent HTTP response.
 */
export type NoContent = Observable<void>;

/**
 * @description
 * Pipe to provide a proper return type of a
 * NoContent HTTP response.
 */
export const NoContentResponse = pipe(map(() => void 0));

export abstract class AbstractApiResource {
  public abstract get<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  public abstract head<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  public abstract post<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  public abstract put<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  public abstract delete<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  /**
   * @description
   * Handle an HTTP response.
   */
  protected handleResponsePipe<T>(): OperatorFunction<
    HttpResponse<T>,
    HttpResponse<T>
  > {
    return pipe(map(this.handleSuccess<T>()), catchError(this.handleError));
  }

  /**
   * @description
   * Handle an HTTP NoContent response.
   */
  protected handleNoContentPipe(): UnaryFunction<NoContent, Observable<void>> {
    return pipe(NoContentResponse, catchError(this.handleError));
  }

  /**
   * @description
   * Handle a successful HTTP response.
   */
  protected handleSuccess<T>(): (response: HttpResponse<T>) => HttpResponse<T> {
    return (response: HttpResponse<T>): HttpResponse<T> => response;
  }

  /**
   * @description
   * Handles getting the result from the response.
   */
  // TODO simplify implementation to use observe
  public unwrapResultPipe<T>(): UnaryFunction<
    Observable<HttpResponse<T>>,
    Observable<T | null>
  > {
    return pipe(map((response: HttpResponse<T>) => response.body));
  }

  /**
   * @description
   * Handle a erroneous HTTP response.
   */
  protected handleError(error: HttpErrorResponse): Observable<never> {
    if (error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(`Backend returned code ${error.status}, body was:`, error);
    }

    return throwError(() => error);
  }
}
