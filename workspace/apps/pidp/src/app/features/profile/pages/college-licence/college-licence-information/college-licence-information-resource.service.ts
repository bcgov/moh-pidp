import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { Observable, catchError } from 'rxjs';

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
  protected apiResource: ApiHttpClient;
  private toastService = inject(ToastService);

  public constructor() {
    const apiResource = inject(ApiHttpClient);

    super(apiResource);
  
    this.apiResource = apiResource;
  }

  public get(partyId: number): Observable<CollegeCertification[] | null> {
    return super.get(partyId).pipe(
      catchError((error: HttpErrorResponse) => {
        this.toastService.openErrorToast(
          'College licence information could not be retrieved',
        );
        throw error;
      }),
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/college-certifications`;
  }
}
