import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import { CrudResource, NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

import { AdministratorInformation } from './administrator-information.model';

@Injectable({
  providedIn: 'root',
})
export class AdministratorInformationResource extends CrudResource<AdministratorInformation> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(id: number, payload: AdministratorInformation): NoContent {
    return super.update(id, payload).pipe(
      tap(() =>
        this.toastService.openSuccessToast(
          'Administrator information has been updated'
        )
      ),
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Administrator information could not be updated'
        );
        return throwError(() => error);
      })
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/access-administrator`;
  }
}
