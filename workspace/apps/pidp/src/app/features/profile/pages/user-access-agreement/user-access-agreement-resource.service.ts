import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, of, tap, throwError } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

@Injectable({
  providedIn: 'root',
})
export class UserAccessAgreementResourceService {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {}

  public acceptAgreement(id: number): NoContent {
    return this.apiResource
      .post<NoContent>(this.getResourcePath(id), null)
      .pipe(
        NoContentResponse,
        tap(() =>
          this.toastService.openSuccessToast(
            'User access agreement has been accepted'
          )
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of(void 0);
          }

          return throwError(() => error);
        })
      );
  }

  protected getResourcePath(id: number): string {
    return `parties/${id}/user-access-agreement`;
  }
}
