import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, tap, throwError } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

@Injectable({
  providedIn: 'root',
})
export class LinkAccountConfirmResource {
  public constructor(
    private apiResource: ApiHttpClient,
    private toastService: ToastService,
  ) {}

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

  public cancelLink(): Observable<unknown> {
    return this.apiResource.delete('credentials/link-ticket/cookie').pipe(
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Something went wrong. Please try again.',
        );
        return throwError(() => error);
      }),
    );
  }
}
