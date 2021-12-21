import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, of } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { LoggerService } from '@core/services/logger.service';
import { ToastService } from '@core/services/toast.service';

import { WorkAndRoleInformationModel } from './work-and-role-information.model';
import { WorkAndRoleInformationModule } from './work-and-role-information.module';

@Injectable({
  providedIn: WorkAndRoleInformationModule,
})
export class WorkAndRoleInformationResource {
  public constructor(
    private apiResource: ApiResource,
    private toastService: ToastService,
    private logger: LoggerService
  ) {}

  public getWorkAndRoleInformation(
    partyId: number
  ): Observable<WorkAndRoleInformationModel | null> {
    return this.apiResource
      .get<WorkAndRoleInformationModel>(`parties/${partyId}/work-setting`)
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

  public updateWorkAndRoleInformation(
    partyId: number,
    workAndRoleInformation: WorkAndRoleInformationModel
  ): NoContent {
    return this.apiResource
      .put<NoContent>(`parties/${partyId}/work-setting`, workAndRoleInformation)
      .pipe(NoContentResponse);
  }
}
