import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { FeedbackSuccessResponse } from './feedback-form-dialog-success.response.model';
import { CrudResource } from '@bcgov/shared/data-access';
import { PersonalInformation } from '@app/features/profile/pages/personal-information/personal-information.model';

@Injectable({
  providedIn: 'root',
})
export class FeedbackFormDialogResource extends CrudResource<PersonalInformation>{
  public constructor(private apiResource: ApiHttpClient) {
    super(apiResource);
  }

  public postFeedback(payload: object): Observable<FeedbackSuccessResponse> {
    return this.apiResource.post<FeedbackSuccessResponse>(
      `feedback`, payload
    );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/demographics`;
  }
}
