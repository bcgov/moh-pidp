import { Injectable } from '@angular/core';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { ToastService } from '@core/services/toast.service';

import { WorkAndRoleInformationModel } from './work-and-role-information.model';

@Injectable()
export class WorkAndRoleInformationResource extends CrudResource<WorkAndRoleInformationModel> {
  public constructor(
    protected apiResource: ApiResource,
    // TODO add in appropriate toasts
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public getResourcePath(partyId: number): string {
    return `parties/${partyId}/college-certification`;
  }
}
