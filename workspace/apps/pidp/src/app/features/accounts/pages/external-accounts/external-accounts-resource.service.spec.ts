import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { ExternalAccountsResource } from './external-accounts-resource.service';

describe('ExternalAccountsResourceService', () => {
  let service: ExternalAccountsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(ApiHttpClient)],
    });
    service = TestBed.inject(ExternalAccountsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
