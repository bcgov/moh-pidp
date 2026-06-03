import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { catchError, tap, throwError } from 'rxjs';

import { CrudResource, NoContent } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ToastService } from '@core/services/toast.service';

import { PersonalInformation } from './personal-information.model';

@Injectable({
  providedIn: 'root',
})
export class PersonalInformationResource extends CrudResource<PersonalInformation> {
  protected apiResource: ApiHttpClient;
  private toastService = inject(ToastService);

  public constructor() {
    const apiResource = inject(ApiHttpClient);

    super(apiResource);
  
    this.apiResource = apiResource;
  }

  public update(id: number, payload: PersonalInformation): NoContent {
    return super.update(id, payload).pipe(
      tap(() =>
        this.toastService.openSuccessToast(
          'Profile information has been updated',
        ),
      ),
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'Profile information could not be updated',
        );
        return throwError(() => error);
      }),
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/demographics`;
  }
}
