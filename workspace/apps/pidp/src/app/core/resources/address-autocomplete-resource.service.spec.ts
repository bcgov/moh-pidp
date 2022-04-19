import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { LoggerService } from '../services/logger.service';
import { ToastService } from '../services/toast.service';
import { AddressAutocompleteResource } from './address-autocomplete-resource.service';
import { ApiHttpClient } from './api-http-client.service';

describe('AddressAutocompleteResource', () => {
  let service: AddressAutocompleteResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AddressAutocompleteResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
        provideAutoSpy(LoggerService),
      ],
    });
    service = TestBed.inject(AddressAutocompleteResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
