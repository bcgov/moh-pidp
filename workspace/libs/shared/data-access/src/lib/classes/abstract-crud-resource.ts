import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

import { Observable, catchError, of } from 'rxjs';

import { AbstractApiResource, NoContent, NoContentResponse } from '.';

export interface IAbstractPageResource<T> {
  create(id: number, payload: T): Observable<T | null>;
  get(id: number): Observable<T | null>;
  update(id: number, payload: T): NoContent;
  delete(id: number, payload: T): Observable<number | null>;
}

export abstract class CrudResource<T> implements IAbstractPageResource<T> {
  protected constructor(protected apiResource: AbstractApiResource) {}

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
    return this.apiResource.post<T>(this.getResourcePath(id), payload).pipe(
      // TODO refactor to use observe
      this.apiResource.unwrapResultPipe(),
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
    return this.apiResource.get<T>(this.getResourcePath(id)).pipe(
      // TODO refactor to use observe
      this.apiResource.unwrapResultPipe(),
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
    return this.apiResource
      .put<NoContent>(this.getResourcePath(id), payload)
      .pipe(NoContentResponse);
  }

  /**
   * @description
   * Delete a resource.
   */
  public delete(id: number, payload: T): Observable<number | null> {
    return this.apiResource
      .delete<number>(this.getResourcePath(id), payload)
      .pipe(
        // TODO refactor to use observe
        this.apiResource.unwrapResultPipe(),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        })
      );
  }
}
