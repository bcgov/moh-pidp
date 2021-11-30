import { Inject, Injectable, Provider } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpStatusCode,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, Observable, of } from 'rxjs';
import { AppConfig, APP_DI_CONFIG } from '../../../app-config.module';

@Injectable()
export class MaintenanceInterceptor implements HttpInterceptor {
  public constructor(
    @Inject(APP_DI_CONFIG) private config: AppConfig,
    private router: Router
  ) {}

  public intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpResponse<Record<string, string>>) => {
        if (error.status === HttpStatusCode.ServiceUnavailable) {
          this.router.navigate([this.config.routes.maintenance]);
        }
        return of(error);
      })
    );
  }
}

export const maintenanceInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: MaintenanceInterceptor,
  multi: true,
  deps: [Router],
};
