import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, throwError } from 'rxjs';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { CollegeCertification } from '../college-licence-declaration/college-certification.model';

@Injectable({
  providedIn: 'root',
})
export class CollegeLicenceInformationResource extends CrudResource<
  CollegeCertification[]
> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public get(partyId: number): Observable<CollegeCertification[] | null> {
    return super.get(partyId).pipe(
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'College licence information could not be retrieved'
        );
        if (error.status === HttpStatusCode.BadRequest) {
          return of([]);
        }

        return throwError(() => error);
      })
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/college-certifications`;
  }
}
