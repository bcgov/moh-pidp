import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import { CrudResource, NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ToastService } from '@core/services/toast.service';

import { WorkAndRoleInformation } from './work-and-role-information.model';

@Injectable()
export class WorkAndRoleInformationResource extends CrudResource<WorkAndRoleInformation> {
  public constructor(
    protected apiResource: ApiHttpClient,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(id: number, payload: WorkAndRoleInformation): NoContent {
    return super.update(id, payload).pipe(
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
    return `parties/${partyId}/work-setting`;
  }
}
