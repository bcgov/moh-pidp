import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HTTP_INTERCEPTORS,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';
import { Router } from '@angular/router';

import { catchError, Observable, of, throwError } from 'rxjs';

@Injectable()
export class UnavailableInterceptor implements HttpInterceptor {
  public constructor(private router: Router) {}

  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpResponse<Record<string, string>>) => {
        if (error.status === HttpStatusCode.ServiceUnavailable) {
          // TODO use root route config to populate redirect
          this.router.navigate(['maintenance']);
          return of(error);
        }

        return throwError(() => error);
      })
    );
  }
}

export const unavailableInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: UnavailableInterceptor,
  multi: true,
  deps: [Router],
};
