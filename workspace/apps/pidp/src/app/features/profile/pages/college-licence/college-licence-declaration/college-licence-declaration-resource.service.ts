import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, tap, throwError } from 'rxjs';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ToastService } from '@core/services/toast.service';

import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

@Injectable()
export class CollegeLicenceDeclarationResource extends CrudResource<PartyLicenceDeclarationInformation> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService,
  ) {
    super(apiResource);
  }

  public updateDeclaration(
    id: number,
    payload: PartyLicenceDeclarationInformation,
  ): Observable<string | null> {
    return this.resource
      .put<string | null>(this.getResourcePath(id), payload)
      .pipe(
        tap(() =>
          this.toastService.openSuccessToast(
            'College licence information has been updated',
          ),
        ),
        catchError((error: HttpErrorResponse) => {
          this.toastService.openErrorToast(
            'College licence information could not be updated',
          );
          return throwError(() => error);
        }),
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/licence-declaration`;
  }
}
