import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, Observable, of } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';
import { ApiResource } from '@core/resources/api-resource.service';
import { LoggerService } from '@core/services/logger.service';
import { ToastService } from '@core/services/toast.service';

import { PersonalInformationModel } from './personal-information.model';

@Injectable({
  providedIn: 'root',
})
export class PersonalInformationResource {
  public constructor(
    private apiResource: ApiResource,
    private toastService: ToastService,
    private logger: LoggerService
  ) {}

  public getPersonalInformation(
    partyId: number
  ): Observable<PersonalInformationModel | null> {
    return this.apiResource
      .get<PersonalInformationModel>(`parties/${partyId}/demographic`)
      .pipe(
        this.apiResource.unwrapResultPipe(),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            return of(null);
          }

          throw error;
        })
      );
  }

  public updatePersonalInformation(
    partyId: number,
    profileInformation: PersonalInformationModel
  ): NoContent {
    return this.apiResource
      .put<NoContent>(`parties/${partyId}/demographic`, profileInformation)
      .pipe(NoContentResponse);
  }
}
