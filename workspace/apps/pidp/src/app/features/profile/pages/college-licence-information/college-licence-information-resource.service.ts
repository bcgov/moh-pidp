import { HttpContext, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import {
  CrudResource,
  NoContent,
  SHOW_LOADING_MESSAGE,
} from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ToastService } from '@core/services/toast.service';

import { CollegeLicenceInformation } from './college-licence-information.model';

@Injectable()
export class CollegeLicenceInformationResource extends CrudResource<CollegeLicenceInformation> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(id: number, payload: CollegeLicenceInformation): NoContent {
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

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/licence-declaration`;
  }
}
