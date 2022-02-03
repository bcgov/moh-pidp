import { TestBed } from '@angular/core/testing';

import { ApiHttpClient } from './api-http-client.service';

describe('ApiHttpClient', () => {
  let service: ApiHttpClient;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiHttpClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
