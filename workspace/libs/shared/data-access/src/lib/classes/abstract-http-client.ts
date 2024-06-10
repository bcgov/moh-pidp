/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import {
  MonoTypeOperatorFunction,
  Observable,
  UnaryFunction,
  catchError,
  map,
  pipe,
  throwError,
} from 'rxjs';

import { IHttpClient } from '../interfaces/http-client.interface';

/**
 * @description
 * Type for indicating a NoContent HTTP response.
 */
export type NoContent = Observable<void>;

/**
 * @description
 * Type for indicating a NoContent HTTP response, but
 * allows for an typed error response.
 */
export type NoContentOrError<T = unknown> = Observable<void | T>;

/**
 * @description
 * Pipe to provide a proper return type of a
 * NoContent HTTP response.
 */
export const NoContentResponse = pipe(map(() => void 0));

export abstract class AbstractHttpClient implements IHttpClient {
  protected abstract uri: string;

  public constructor(protected http: HttpClient) {}

  public get<T>(
    path: string,
    options?: { [key: string]: unknown },
  ): Observable<T> {
    return this.http
      .get<T>(`${this.uri}/${path}`, {
        ...options,
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public head<T>(
    path: string,
    options?: { [key: string]: unknown },
  ): Observable<T> {
    return this.http
      .head<T>(`${this.uri}/${path}`, {
        ...options,
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public post<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: unknown },
  ): Observable<T> {
    return this.http
      .post<T>(`${this.uri}/${path}`, body, {
        ...options,
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public put<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: unknown },
  ): Observable<T> {
    return this.http
      .put<T>(`${this.uri}/${path}`, body, {
        ...options,
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public delete<T>(
    path: string,
    options?: { [key: string]: unknown },
  ): Observable<T> {
    return this.http
      .delete<T>(`${this.uri}/${path}`, {
        ...options,
      })
      .pipe(this.handleResponsePipe<T>());
  }

  /**
   * @description
   * Handle an HTTP response.
   */
  protected handleResponsePipe<T>(): MonoTypeOperatorFunction<T> {
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
  protected handleSuccess<T>(): (response: T) => T {
    return (response: T): T => response;
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
