import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';

import { Observable, retryWhen, tap } from 'rxjs';

/**
 * @description
 * Maximum retry attempts when an HTTP error occurs.
 */
export const MAX_HTTP_RETRY_ATTEMPTS = 1;

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {
  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    let httpRetryAttempts = 0;
    return next.handle(request).pipe(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      retryWhen((errors: Observable<any>) => {
        return errors.pipe(
          tap((error: HttpErrorResponse) => {
            if (
              error.headers.get('No-Retry') ||
              error.status.toString().startsWith('5') ||
              httpRetryAttempts >= MAX_HTTP_RETRY_ATTEMPTS
            ) {
              // Prevent retry by forcing entry into error handler
              throw error;
            }

            httpRetryAttempts++;
          }),
        );
      }),
    );
  }
}

export const serverErrorInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ServerErrorInterceptor,
  multi: true,
};
