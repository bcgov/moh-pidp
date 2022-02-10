import { Observable } from 'rxjs';

import { NoContent } from '../classes/abstract-http-client';

export interface ICrudResource<T> {
  create(id: number, payload: T): Observable<T | null>;
  get(id: number): Observable<T | null>;
  update(id: number, payload: T): NoContent;
  delete(id: number, payload: T): Observable<number | null>;
}
