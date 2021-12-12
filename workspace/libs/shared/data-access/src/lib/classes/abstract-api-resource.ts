import {
  Observable,
  throwError,
  pipe,
  UnaryFunction,
  OperatorFunction,
} from 'rxjs';

import { map, catchError } from 'rxjs/operators';

import { ApiHttpErrorResponse, ApiHttpResponse } from '.';

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
    options: { [key: string]: unknown }
  ): Observable<ApiHttpResponse<T>>;

  public abstract head<T>(
    path: string,
    options: { [key: string]: unknown }
  ): Observable<ApiHttpResponse<T>>;

  public abstract post<T>(
    path: string,
    body: { [key: string]: unknown },
    options: { [key: string]: unknown }
  ): Observable<ApiHttpResponse<T>>;

  public abstract put<T>(
    path: string,
    body: { [key: string]: unknown },
    options: { [key: string]: unknown }
  ): Observable<ApiHttpResponse<T>>;

  public abstract delete<T>(
    path: string,
    options: { [key: string]: unknown }
  ): Observable<ApiHttpResponse<T>>;

  /**
   * @description
   * Handle an HTTP response.
   */
  protected handleResponsePipe<T>(): OperatorFunction<
    ApiHttpResponse<T>,
    ApiHttpResponse<T>
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
  protected handleSuccess<T>(): (
    response: ApiHttpResponse<T>
  ) => ApiHttpResponse<T> {
    return (response: ApiHttpResponse<T>): ApiHttpResponse<T> => response;
  }

  /**
   * @description
   * Handle a erroneous HTTP response.
   */
  protected handleError(error: ApiHttpErrorResponse): Observable<never> {
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
