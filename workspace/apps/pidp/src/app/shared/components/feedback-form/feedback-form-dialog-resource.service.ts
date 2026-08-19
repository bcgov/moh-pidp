import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';

import { CrudResource } from '@bcgov/shared/data-access';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PersonalInformation } from '@app/features/profile/pages/personal-information/personal-information.model';

import { FeedbackSuccessResponse } from './feedback-form-dialog-success.response.model';

@Injectable({
  providedIn: 'root',
})
export class FeedbackFormDialogResource extends CrudResource<PersonalInformation> {
  private readonly apiResource: ApiHttpClient;

  public constructor() {
    const apiResource = inject(ApiHttpClient);

    super(apiResource);
  
    this.apiResource = apiResource;
  }

  public postFeedback(payload: object): Observable<FeedbackSuccessResponse> {
    return this.apiResource.post<FeedbackSuccessResponse>(`feedback`, payload);
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/demographics`;
  }
}
