import { Observable } from 'rxjs';

import { NoContent } from '../classes/abstract-http-client';

export interface ICrudResource<T> {
  create(
    id: number,
    payload: T,
    options?: { [key: string]: unknown },
  ): Observable<T | null>;
  get(id: number, options?: { [key: string]: unknown }): Observable<T | null>;
  update(
    id: number,
    payload: T,
    options?: { [key: string]: unknown },
  ): NoContent;
  delete(
    id: number,
    options?: { [key: string]: unknown },
  ): Observable<number | null>;
}
