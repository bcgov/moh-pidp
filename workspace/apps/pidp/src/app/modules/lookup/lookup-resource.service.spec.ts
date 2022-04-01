import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { LookupResource } from './lookup-resource.service';

describe('LookupResource', () => {
  let service: LookupResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LookupResource, provideAutoSpy(ApiHttpClient)],
    });

    service = TestBed.inject(LookupResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
