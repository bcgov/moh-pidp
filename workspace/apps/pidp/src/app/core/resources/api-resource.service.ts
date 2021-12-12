import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import {
  AbstractApiResource,
  ApiHttpResponse,
} from '@bcgov/shared/data-access';
import { LoggerService } from '@core/services/logger.service';

@Injectable({
  providedIn: 'root',
})
export class ApiResource extends AbstractApiResource {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private http: HttpClient,
    private logger: LoggerService
  ) {
    super();
  }

  public get<T>(
    path: string,
    options: { [key: string]: unknown } = {}
  ): Observable<ApiHttpResponse<T>> {
    return this.http
      .get<T>(`${this.config.apiEndpoint}/${path}`, {
        ...options,
        // TODO make body the default and account for change in typings using
        //      overloads then allow this to be overwritten
        observe: 'response',
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public head<T>(
    path: string,
    options: { [key: string]: unknown } = {}
  ): Observable<ApiHttpResponse<T>> {
    return this.http
      .head<T>(`${this.config.apiEndpoint}/${path}`, {
        ...options,
        // TODO make body the default and account for change in typings using
        //      overloads then allow this to be overwritten
        observe: 'response',
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public post<T>(
    path: string,
    body: { [key: string]: unknown } = {},
    options: { [key: string]: unknown } = {}
  ): Observable<ApiHttpResponse<T>> {
    return this.http
      .post<T>(`${this.config.apiEndpoint}/${path}`, body, {
        ...options,
        // TODO make body the default and account for change in typings using
        //      overloads then allow this to be overwritten
        observe: 'response',
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public put<T>(
    path: string,
    body: { [key: string]: unknown } = {},
    options: { [key: string]: unknown } = {}
  ): Observable<ApiHttpResponse<T>> {
    return this.http
      .put<T>(`${this.config.apiEndpoint}/${path}`, body, {
        ...options,
        // TODO make body the default and account for change in typings using
        //      overloads then allow this to be overwritten
        observe: 'response',
      })
      .pipe(this.handleResponsePipe<T>());
  }

  public delete<T>(
    path: string,
    options: { [key: string]: unknown } = {}
  ): Observable<ApiHttpResponse<T>> {
    return this.http
      .delete<T>(`${this.config.apiEndpoint}/${path}`, {
        ...options,
        // TODO make body the default and account for change in typings using
        //      overloads then allow this to be overwritten
        observe: 'response',
      })
      .pipe(this.handleResponsePipe<T>());
  }

  /**
   * @description
   * Handle a successful HTTP response.
   */
  protected handleSuccess<T>(): (
    response: ApiHttpResponse<T>
  ) => ApiHttpResponse<T> {
    return (response: ApiHttpResponse<T>): ApiHttpResponse<T> => {
      this.logger.info(`RESPONSE: ${response.status}`, response.body);

      return response;
    };
  }
}
