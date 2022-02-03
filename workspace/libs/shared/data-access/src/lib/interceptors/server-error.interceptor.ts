import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';

import { Observable, retry } from 'rxjs';

/**
 * @description
 * Retry attempts when an HTTP error occurs.
 */
export const HTTP_RETRY_ATTEMPTS = 1;

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {
  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(retry(HTTP_RETRY_ATTEMPTS));
  }
}

export const serverErrorInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ServerErrorInterceptor,
  multi: true,
};
