import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

import { Observable, catchError, of } from 'rxjs';

import { ICrudResource } from '../interfaces/crud-resource.interface';
import {
  AbstractResource,
  NoContent,
  NoContentResponse,
} from './abstract-resource';

export abstract class CrudResource<T> implements ICrudResource<T> {
  protected constructor(protected resource: AbstractResource) {}

  /**
   * @description
   * Provides the resource path with interpolated identifier.
   */
  abstract getResourcePath(id: number): string;

  /**
   * @description
   * Create a new resource.
   */
  public create(id: number, payload: T): Observable<T | null> {
    return this.resource.post<T>(this.getResourcePath(id), payload).pipe(
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    );
  }

  /**
   * @description
   * Read a resource.
   */
  public get(id: number): Observable<T | null> {
    return this.resource.get<T>(this.getResourcePath(id)).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      })
    );
  }

  /**
   * @description
   * Update a resource.
   */
  public update(id: number, payload: T): NoContent {
    return this.resource
      .put<NoContent>(this.getResourcePath(id), payload)
      .pipe(NoContentResponse);
  }

  /**
   * @description
   * Delete a resource.
   */
  public delete(id: number): Observable<number | null> {
    return this.resource.delete<number>(this.getResourcePath(id)).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          return of(null);
        }

        throw error;
      })
    );
  }
}
