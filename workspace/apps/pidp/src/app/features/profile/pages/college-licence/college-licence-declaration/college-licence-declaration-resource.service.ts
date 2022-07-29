import {
  HttpContext,
  HttpErrorResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of, tap, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  SHOW_LOADING_MESSAGE,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ToastService } from '@core/services/toast.service';

import { CollegeCertification } from './college-certification.model';
import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

@Injectable()
export class CollegeLicenceDeclarationResource extends CrudResource<PartyLicenceDeclarationInformation> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(
    id: number,
    payload: PartyLicenceDeclarationInformation
  ): NoContent {
    return super
      .update(id, payload, {
        context: new HttpContext().set(SHOW_LOADING_MESSAGE, true),
      })
      .pipe(
        tap(() =>
          this.toastService.openSuccessToast(
            'College licence information has been updated'
          )
        ),
        catchError((error: HttpErrorResponse) => {
          this.toastService.openErrorToast(
            'College licence information could not be updated'
          );
          return throwError(() => error);
        })
      );
  }

  public getCollegeCertifications(
    partyId: number
  ): Observable<CollegeCertification[]> {
    return this.apiResource
      .get<CollegeCertification[]>(this.getCertificationResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.BadRequest) {
            return of([]);
          }

          return throwError(() => error);
        })
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/licence-declaration`;
  }

  protected getCertificationResourcePath(partyId: number): string {
    return `parties/${partyId}/college-certifications`;
  }
}
