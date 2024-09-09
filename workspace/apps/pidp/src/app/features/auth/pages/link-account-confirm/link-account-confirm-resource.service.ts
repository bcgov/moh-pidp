import { HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { Observable, catchError, map, tap, throwError } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class LinkAccountConfirmResource {
  public logoutRedirectUrl: string;
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private apiResource: ApiHttpClient,
    private authService: AuthService,
    private toastService: ToastService,
  ) {
    this.logoutRedirectUrl = `${this.config.applicationUrl}/`;
  }

  public linkAccount(): Observable<number> {
    return this.apiResource.post<number>('credentials', {}).pipe(
      tap(() =>
        this.toastService.openErrorToast(
          'Your account was successfully linked.',
        ),
      ),
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Your account could not be linked. Please try again.',
        );
        return throwError(() => error);
      }),
    );
  }

  public cancelLink(): Observable<Observable<void> | boolean> {
    return this.apiResource.delete('credentials/link-ticket/cookie').pipe(
      map(() => this.authService.logout(this.logoutRedirectUrl)),
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Something went wrong. Please try again.',
        );
        return throwError(() => error);
      }),
    );
  }
}
