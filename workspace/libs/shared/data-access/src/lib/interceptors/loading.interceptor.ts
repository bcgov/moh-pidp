import {
  HTTP_INTERCEPTORS,
  HttpContextToken,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';

import { Observable, finalize } from 'rxjs';

import { LoadingService } from '../services/loading.service';

/**
 * @description
 * Provided by HTTP requests indicating whether the
 * loading service should show a message in the overlay.
 */
export const SHOW_LOADING_MESSAGE = new HttpContextToken(() => false);

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private totalRequests: number;

  public constructor(private loadingService: LoadingService) {
    this.totalRequests = 0;
  }

  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    const withMessage = request.context.get(SHOW_LOADING_MESSAGE);

    this.totalRequests++;
    this.loadingService.show(withMessage);

    return next.handle(request).pipe(
      finalize(() => {
        this.totalRequests--;

        if (this.totalRequests <= 0) {
          this.loadingService.hide();
        }
      }),
    );
  }
}

export const loadingInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: LoadingInterceptor,
  multi: true,
};
