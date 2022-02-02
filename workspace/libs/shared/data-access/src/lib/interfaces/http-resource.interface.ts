/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpResponse } from '@angular/common/http';

import { Observable } from 'rxjs';

// TODO rename from resource to IHttpClient
export interface IHttpResource {
  get<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  head<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  post<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  put<T>(
    path: string,
    body: { [key: string]: any } | null,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;

  delete<T>(
    path: string,
    options?: { [key: string]: any }
  ): Observable<HttpResponse<T>>;
}
