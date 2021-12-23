import { Injectable } from '@angular/core';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiResource } from '@core/resources/api-resource.service';
import { ToastService } from '@core/services/toast.service';

import { PersonalInformationModel } from './personal-information.model';

@Injectable()
export class PersonalInformationResource extends CrudResource<PersonalInformationModel> {
  public constructor(
    protected apiResource: ApiResource,
    // TODO add in appropriate toasts
    private toastService: ToastService
  ) {
    super(apiResource);
  }

  public getResourcePath(partyId: number): string {
    return `parties/${partyId}/demographics`;
  }
}
