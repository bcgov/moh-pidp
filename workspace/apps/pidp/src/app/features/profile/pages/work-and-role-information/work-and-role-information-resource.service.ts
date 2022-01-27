import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import { CrudResource, NoContent } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { ToastService } from '@core/services/toast.service';

import { WorkAndRoleInformationModel } from './work-and-role-information.model';

@Injectable()
export class WorkAndRoleInformationResource extends CrudResource<WorkAndRoleInformationModel> {
  public constructor(
    protected apiResource: ApiResource,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(id: number, payload: WorkAndRoleInformationModel): NoContent {
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
