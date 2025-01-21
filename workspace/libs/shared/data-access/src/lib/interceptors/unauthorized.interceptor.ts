import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';
import { Router } from '@angular/router';

import { Observable, catchError, of, throwError } from 'rxjs';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
  public constructor(private readonly router: Router) {}

  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpResponse<Record<string, string>>) => {
        if (error.status === HttpStatusCode.Unauthorized) {
          // Allow route configuration to determine destination
          // TO DO use root route config to populate redirect
          this.router.navigate(['/']);
          return of(error);
        }

        return throwError(() => error);
      }),
    );
  }
}

export const unauthorizedInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: UnauthorizedInterceptor,
  multi: true,
  deps: [Router],
};
