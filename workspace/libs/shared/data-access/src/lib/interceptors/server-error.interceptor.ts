import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';

import { Observable, retry } from 'rxjs';

/**
 * @description
 * Retry attempts when an HTTP error occurs.
 */
// TODO create an injector token for this constant
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
