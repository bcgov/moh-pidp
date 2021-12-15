import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { NoContent, NoContentResponse } from '@bcgov/shared/data-access';
import { ApiResource } from '@core/resources/api-resource.service';

import { PersonalInformationModel } from './personal-information.model';

@Injectable({
  providedIn: 'root',
})
export class ProfileInformationResource {
  public constructor(private apiResource: ApiResource) {}

  public getProfileInformation(
    partyId: number
  ): Observable<PersonalInformationModel | null> {
    return this.apiResource
      .get<PersonalInformationModel>(`parties/${partyId}/demographic`)
      .pipe(this.apiResource.unwrapResultPipe());
  }

  public updateProfileInformation(
    partyId: number,
    profileInformation: PersonalInformationModel
  ): Observable<NoContent> {
    return this.apiResource
      .put(`parties/${partyId}/demographic`, profileInformation)
      .pipe(NoContentResponse);
  }
}
