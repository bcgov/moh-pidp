import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import { CrudResource, NoContent } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { ToastService } from '@core/services/toast.service';

import { PersonalInformationModel } from './personal-information.model';

@Injectable()
export class PersonalInformationResource extends CrudResource<PersonalInformationModel> {
  public constructor(
    protected apiResource: ApiResource,
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public update(id: number, payload: PersonalInformationModel): NoContent {
    return super.update(id, payload).pipe(
      tap(() =>
        this.toastService.openSuccessToast(
          'Profile information has been updated'
        )
      ),
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Profile information could not be updated'
        );
        return throwError(() => error);
      })
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/demographics`;
  }
}
