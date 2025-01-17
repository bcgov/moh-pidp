import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { FeedbackSuccessResponse } from './feedback-form-dialog-success.response.model';

@Injectable({
  providedIn: 'root',
})
export class FeedbackFormDialogResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public postFeedback(payload: object): Observable<FeedbackSuccessResponse> {
    return this.apiResource.post<FeedbackSuccessResponse>(
      `feedback`, payload
    );
  }

}
