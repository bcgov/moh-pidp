import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

import { Observable, catchError, of } from 'rxjs';

import { ICrudResource } from '../interfaces/crud-resource.interface';
import {
  AbstractHttpClient,
  NoContent,
  NoContentResponse,
} from './abstract-http-client';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export abstract class CrudResource<T extends { [key: string]: any } | null>
  implements ICrudResource<T>
{
  protected constructor(protected resource: AbstractHttpClient) {}

  /**
   * @description
   * Create a new resource.
   */
  public create(
    id: number,
    payload: T,
    options?: { [key: string]: unknown },
  ): Observable<T | null> {
    return this.resource
      .post<T>(this.getResourcePath(id), payload, options)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          throw error;
        }),
      );
  }

  /**
   * @description
   * Read a resource.
   */
  public get(
    id: number,
    options?: { [key: string]: unknown },
  ): Observable<T | null> {
    return this.resource.get<T>(this.getResourcePath(id), options).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      }),
    );
  }

  /**
   * @description
   * Update a resource.
   */
  public update(
    id: number,
    payload: T,
    options?: { [key: string]: unknown },
  ): NoContent {
    return this.resource
      .put<NoContent>(this.getResourcePath(id), payload, options)
      .pipe(NoContentResponse);
  }

  /**
   * @description
   * Delete a resource.
   */
  public delete(
    id: number,
    options?: { [key: string]: unknown },
  ): Observable<number | null> {
    return this.resource.delete<number>(this.getResourcePath(id), options).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      }),
    );
  }

  /**
   * @description
   * Provides the resource path with interpolated identifier.
   */
  protected abstract getResourcePath(id: number): string;
}
