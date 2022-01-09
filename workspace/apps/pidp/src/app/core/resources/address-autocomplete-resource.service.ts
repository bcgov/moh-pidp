import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, catchError, map, of } from 'rxjs';

import { AddressAutocompleteFindResponse } from '@shared/components/address-autocomplete/address-autocomplete-find-response.model';
import { AddressAutocompleteRetrieveResponse } from '@shared/components/address-autocomplete/address-autocomplete-retrieve-response.model';

import { ApiResource } from '../resources/api-resource.service';
import { LoggerService } from '../services/logger.service';
import { ToastService } from '../services/toast.service';

@Injectable({
  providedIn: 'root',
})
export class AddressAutocompleteResource {
  public constructor(
    private apiResource: ApiResource,
    private toastService: ToastService,
    private logger: LoggerService
  ) {}

  public find(
    searchTerm: string
  ): Observable<AddressAutocompleteFindResponse[]> {
    return this.apiResource
      .get<AddressAutocompleteFindResponse[]>(
        `AddressAutocomplete/find?searchTerm=${searchTerm}`
      )
      .pipe(
        map(
          (response: HttpResponse<AddressAutocompleteFindResponse[]>) =>
            response.body ?? []
        ),
        catchError((error: unknown) => {
          this.toastService.openErrorToast('Address could not be found');
          this.logger.error(
            '[AddressAutocompleteResource::find] error has occurred: ',
            error
          );

          return of([]);
        })
      );
  }

  public retrieve(
    id: string
  ): Observable<AddressAutocompleteRetrieveResponse[]> {
    return this.apiResource
      .get<AddressAutocompleteRetrieveResponse[]>(
        `AddressAutocomplete/retrieve?id=${id}`
      )
      .pipe(
        map(
          (response: HttpResponse<AddressAutocompleteRetrieveResponse[]>) =>
            response.body ?? []
        ),
        catchError((error: unknown) => {
          this.toastService.openErrorToast('Address could not be retrieved');
          this.logger.error(
            '[AddressAutocompleteResource::retrieve] error has occurred: ',
            error
          );

          return of([]);
        })
      );
  }
}
