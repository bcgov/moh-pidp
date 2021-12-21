import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { LoggerService } from '@core/services/logger.service';
import { ToastService } from '@core/services/toast.service';

import { CollegeLicenceInformationModel } from './college-licence-information.model';
import { CollegeLicenceInformationModule } from './college-licence-information.module';

@Injectable({
  providedIn: CollegeLicenceInformationModule,
})
export class CollegeLicenceInformationResource {
  public constructor(
    private apiResource: ApiResource,
    private toastService: ToastService,
    private logger: LoggerService
  ) {}

  public getCollegeLicenceInformation(
    partyId: number
  ): Observable<CollegeLicenceInformationModel | null> {
    return this.apiResource
      .get<CollegeLicenceInformationModel>(
        `parties/${partyId}/college-certificate`
      )
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

  public updateCollegeLicenceInformation(
    partyId: number,
    collegeLicenceInformationModel: CollegeLicenceInformationModel
  ): NoContent {
    return this.apiResource
      .put<NoContent>(
        `parties/${partyId}/college-certificate`,
        collegeLicenceInformationModel
      )
      .pipe(NoContentResponse);
  }
}
